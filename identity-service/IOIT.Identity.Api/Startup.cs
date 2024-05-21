using AutoMapper;
using IOIT.Identity.Api.Consumers;
using IOIT.Identity.Api.Extensions;
using IOIT.Identity.Api.Filters;
using IOIT.Identity.Api.Options;
using IOIT.Identity.Api.Producers;
using IOIT.Identity.Api.Services;
using IOIT.Identity.Application;
using IOIT.Identity.Application.Common.Interfaces;
using IOIT.Identity.Application.Common.Interfaces.Producer;
using IOIT.Identity.Application.Interfaces;
using IOIT.Identity.Infrastructure;
using IOIT.Shared.Queues;
using IOIT.Shared.ViewModels.DtoQueues;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RabbitMQ.Client;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using static IOIT.Shared.Queues.UtilitiesServiceQueues;

namespace IOIT.Identity.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(env.ContentRootPath)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }
        public static string ContentRootPath { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //GlobalDiagnosticsContext.Set("configDir", Configuration.GetConnectionString("FileSavedLogs"));
            //GlobalDiagnosticsContext.Set("connectionString", Configuration.GetConnectionString("DbLogging"));
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<INotificationProducer, NotificationProducers>();
            services.AddTransient<IIdentityProducer, IdentityProducers>();

            services.AddControllersWithViews(options =>
                options.Filters.Add(typeof(GlobalExceptionFilterAttribute))).AddNewtonsoftJson(
                (options =>
                     options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore));

            services.AddOptions();
            services.AddLogging();

            services.Configure<AppOptions>(Configuration);
            //services.AddCors(c =>
            //{
            //    c.AddPolicy("AllowCors", options =>
            //    options
            //    .WithOrigins("http://.tnsplus.vn", "https://*.tnsplus.vn", "https://*.brandsms.vn", "http://10.254.64.30", "http://10.254.25.102", "http://localhost:8088", "http://localhost:8081", "http://10.254.64.128", "http://10.254.64.129")
            //    .SetIsOriginAllowedToAllowWildcardSubdomains()
            //    .AllowAnyMethod()
            //    .AllowCredentials()
            //    .AllowAnyHeader()
            //    .Build()
            //    );
            //});
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", builder =>
                {
                    builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
                });
            });

            services.AddMvc(options =>
            {
                var noContentFormatter = options.OutputFormatters.OfType<HttpNoContentOutputFormatter>().FirstOrDefault();
                if (noContentFormatter != null)
                {
                    noContentFormatter.TreatNullValueAsNoContent = false;
                }
            });

            var coreMappingAssembly = typeof(Application.Models.Mappings.AutoMapping).Assembly;
            services.AddAutoMapper(coreMappingAssembly);

            services.AddHttpContextAccessor();
            services.AddTransient<ICurrentAdminService<Guid>, CurrentUserService<Guid>>();
            services.RegisterApplicationService();
            services.RegisterInfrastructureServices(Configuration);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "QLCD API V1",
                    Description = "APIs are built for QLCD system by IOIT",
                    //TermsOfService = new Uri("https://example.com/terms")
                });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            new string[] {}
                    }
                });
            });

            services.AddHttpClient();

            services.AddMassTransit(x => {

                x.AddConsumer<CreateProjectConfirmedConsumer>();
                x.AddConsumer<UpdateProjectConfirmedConsumer>();
                x.AddConsumer<CreateFloorConfirmedConsumer>();
                x.AddConsumer<UpdateFloorConfirmedConsumer>();
                x.AddConsumer<CreateTowerConfirmedConsumer>();
                x.AddConsumer<UpdateTowerConfirmedConsumer>();
                x.AddConsumer<CreateApartmentConfirmedConsumer>();
                x.AddConsumer<UpdateApartmentConfirmedConsumer>();
                x.AddConsumer<CreateDepartmentConfirmedConsumer>();
                x.AddConsumer<UpdateDepartmentConfirmedConsumer>();
                x.AddConsumer<CreateTypeAttributeItemConfirmedConsumer>();
                x.AddConsumer<UpdateTypeAttributeItemConfirmedConsumer>();
                x.AddConsumer<UtilitiesResidentUpdateConsumer>();
                x.AddConsumer<UtilitiesResidentUpdateStatusConsumer>();
                x.AddConsumer<UpdateUserInfoFirebaseConsumer>();
                x.AddConsumer<CommonApartmentMapConsumer>();

                x.UsingRabbitMq((ctx, cfg) => {
                    cfg.Host(Configuration["RabbitMQConnection:HostAddress"],
                        5672,
                        "/",
                        h =>
                        {
                            h.Username(Configuration["RabbitMQConnection:UserName"]);
                            h.Password(Configuration["RabbitMQConnection:Password"]);
                        });

                    cfg.UseHealthCheck(ctx);

                    cfg.ReceiveEndpoint(CommonServiceQueues.CommonProjectCreatedQueue.NameIdentityQueue, c =>
                    {
                        c.ConfigureConsumer<CreateProjectConfirmedConsumer>(ctx);
                        c.Bind(CommonServiceQueues.CommonProjectCreatedQueue.NameExchange, x =>
                        {
                            x.ExchangeType = ExchangeType.Fanout;
                        });
                    });

                    cfg.ReceiveEndpoint(CommonServiceQueues.CommonProjectUpdatedQueue.NameIdentityQueue, c =>
                    {
                        c.ConfigureConsumer<UpdateProjectConfirmedConsumer>(ctx);
                        c.Bind(CommonServiceQueues.CommonProjectUpdatedQueue.NameExchange, x =>
                        {
                            x.ExchangeType = ExchangeType.Fanout;
                        });
                    });

                    cfg.ReceiveEndpoint(CommonServiceQueues.CommonTowerCreatedQueue.NameIdentityQueue, c =>
                    {
                        c.ConfigureConsumer<CreateTowerConfirmedConsumer>(ctx);
                        c.Bind(CommonServiceQueues.CommonTowerCreatedQueue.NameExchange, x =>
                        {
                            x.ExchangeType = ExchangeType.Fanout;
                        });
                    });

                    cfg.ReceiveEndpoint(CommonServiceQueues.CommonTowerUpdatedQueue.NameIdentityQueue, c =>
                    {
                        c.ConfigureConsumer<UpdateTowerConfirmedConsumer>(ctx);
                        c.Bind(CommonServiceQueues.CommonTowerUpdatedQueue.NameExchange, x =>
                        {
                            x.ExchangeType = ExchangeType.Fanout;
                        });
                    });

                    cfg.ReceiveEndpoint(CommonServiceQueues.CommonFloorCreatedQueue.NameIdentityQueue, c =>
                    {
                        c.ConfigureConsumer<CreateFloorConfirmedConsumer>(ctx);
                        c.Bind(CommonServiceQueues.CommonFloorCreatedQueue.NameExchange, x =>
                        {
                            x.ExchangeType = ExchangeType.Fanout;
                        });
                    });

                    cfg.ReceiveEndpoint(CommonServiceQueues.CommonFloorUpdatedQueue.NameIdentityQueue, c =>
                    {
                        c.ConfigureConsumer<UpdateFloorConfirmedConsumer>(ctx);
                        c.Bind(CommonServiceQueues.CommonFloorUpdatedQueue.NameExchange, x =>
                        {
                            x.ExchangeType = ExchangeType.Fanout;
                        });
                    });

                    cfg.ReceiveEndpoint(CommonServiceQueues.CommonApartmentCreatedQueue.NameIdentityQueue, c =>
                    {
                        c.ConfigureConsumer<CreateApartmentConfirmedConsumer>(ctx);
                        c.Bind(CommonServiceQueues.CommonApartmentCreatedQueue.NameExchange, x =>
                        {
                            x.ExchangeType = ExchangeType.Fanout;
                        });
                    });

                    cfg.ReceiveEndpoint(CommonServiceQueues.CommonApartmentUpdatedQueue.NameIdentityQueue, c =>
                    {
                        c.ConfigureConsumer<UpdateApartmentConfirmedConsumer>(ctx);
                        c.Bind(CommonServiceQueues.CommonApartmentUpdatedQueue.NameExchange, x =>
                        {
                            x.ExchangeType = ExchangeType.Fanout;
                        });
                    });

                    cfg.ReceiveEndpoint(CommonServiceQueues.CommonDepartmentCreatedQueue.NameIdentityQueue, c =>
                    {
                        c.ConfigureConsumer<CreateDepartmentConfirmedConsumer>(ctx);
                        c.Bind(CommonServiceQueues.CommonDepartmentCreatedQueue.NameExchange, x =>
                        {
                            x.ExchangeType = ExchangeType.Fanout;
                        });
                    });

                    cfg.ReceiveEndpoint(CommonServiceQueues.CommonDepartmentUpdatedQueue.NameIdentityQueue, c =>
                    {
                        c.ConfigureConsumer<UpdateDepartmentConfirmedConsumer>(ctx);
                        c.Bind(CommonServiceQueues.CommonDepartmentUpdatedQueue.NameExchange, x =>
                        {
                            x.ExchangeType = ExchangeType.Fanout;
                        });
                    });

                    cfg.ReceiveEndpoint(CommonServiceQueues.CommonTypeAttributeItemCreatedQueue.NameIdentityQueue, c =>
                    {
                        c.ConfigureConsumer<CreateTypeAttributeItemConfirmedConsumer>(ctx);
                        c.Bind(CommonServiceQueues.CommonTypeAttributeItemCreatedQueue.NameExchange, x =>
                        {
                            x.ExchangeType = ExchangeType.Fanout;
                        });
                    });

                    cfg.ReceiveEndpoint(CommonServiceQueues.CommonTypeAttributeItemUpdatedQueue.NameIdentityQueue, c =>
                    {
                        c.ConfigureConsumer<UpdateTypeAttributeItemConfirmedConsumer>(ctx);
                        c.Bind(CommonServiceQueues.CommonTypeAttributeItemUpdatedQueue.NameExchange, x =>
                        {
                            x.ExchangeType = ExchangeType.Fanout;
                        });
                    });

                    cfg.ReceiveEndpoint(UtilitiesResidentUpdateStatusQueue.NameIdentityQueue, c =>
                    {
                        c.ConfigureConsumer<UtilitiesResidentUpdateStatusConsumer>(ctx);
                        c.Bind(UtilitiesResidentUpdateStatusQueue.NameExchange, x =>
                        {
                            x.ExchangeType = ExchangeType.Fanout;
                        });
                    });

                    cfg.ReceiveEndpoint(UtilitiesResidentUpdateQueue.NameIdentityQueue, c =>
                    {
                        c.ConfigureConsumer<UtilitiesResidentUpdateConsumer>(ctx);
                        c.Bind(UtilitiesResidentUpdateQueue.NameExchange, x =>
                        {
                            x.ExchangeType = ExchangeType.Fanout;
                        });
                    });

                    cfg.ReceiveEndpoint(CommonServiceQueues.CommonApartmentMapMapQueue.NameIdentityQueue, c =>
                    {
                        c.ConfigureConsumer<CommonApartmentMapConsumer>(ctx);
                        c.Bind(CommonServiceQueues.CommonApartmentMapMapQueue.NameExchange, x =>
                        {
                            x.ExchangeType = ExchangeType.Fanout;
                        });
                    });

                    cfg.ReceiveEndpoint(InvoiceServiceQueues.InvoiceUpdateUserInfoFirebaseQueue.Name, c =>
                    {
                        c.ConfigureConsumer<UpdateUserInfoFirebaseConsumer>(ctx);
                    });

                    //Producers
                    cfg.Message<DtoCommonEmployeeQueue>(e => e.SetEntityName(IdentityServiceQueues.CommonEmployeeQueue.NameExchange));
                    cfg.Publish<DtoCommonEmployeeQueue>(x =>
                    {
                        x.ExchangeType = ExchangeType.Fanout; // default, allows any valid exchange type
                    });

                    cfg.Message<DtoCommonEmployeeMapQueue>(e => e.SetEntityName(IdentityServiceQueues.CommonEmployeeMapQueue.NameExchange));
                    cfg.Publish<DtoCommonEmployeeMapQueue>(x =>
                    {
                        x.ExchangeType = ExchangeType.Fanout; // default, allows any valid exchange type
                    });

                    cfg.Message<DtoCommonEmployeeMapUpdatedQueue>(e => e.SetEntityName(IdentityServiceQueues.CommonEmployeeMapUpdatedQueue.NameExchange));
                    cfg.Publish<DtoCommonEmployeeMapUpdatedQueue>(x =>
                    {
                        x.ExchangeType = ExchangeType.Fanout; // default, allows any valid exchange type
                    });

                    cfg.Message<DtoCommonUserQueue>(e => e.SetEntityName(IdentityServiceQueues.CommonUserQueue.NameExchange));
                    cfg.Publish<DtoCommonUserQueue>(x =>
                    {
                        x.ExchangeType = ExchangeType.Fanout; // default, allows any valid exchange type
                    });

                    cfg.Message<DtoCommonResidentQueue>(e => e.SetEntityName(IdentityServiceQueues.CommonResidentQueue.NameExchange));
                    cfg.Publish<DtoCommonResidentQueue>(x =>
                    {
                        x.ExchangeType = ExchangeType.Fanout; // default, allows any valid exchange type
                    });

                    cfg.Message<DtoCommonResidentUpdateQueue>(e => e.SetEntityName(IdentityServiceQueues.CommonResidentUpdateQueue.NameExchange));
                    cfg.Publish<DtoCommonResidentUpdateQueue>(x =>
                    {
                        x.ExchangeType = ExchangeType.Fanout; // default, allows any valid exchange type
                    });

                    cfg.Message<DtoIdentityApartmentMapQueue>(e => e.SetEntityName(IdentityServiceQueues.IdentityApartmentMapQueue.NameExchange));
                    cfg.Publish<DtoIdentityApartmentMapQueue>(x =>
                    {
                        x.ExchangeType = ExchangeType.Fanout; // default, allows any valid exchange type
                    });
                });
            });


            services.AddMassTransitHostedService();

            //services.AddTokenAuthentication(Configuration);
            
            string domain = Configuration["AppSettings:JwtIssuer"];
            var authenticationProviderKey = "TestKey";
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = authenticationProviderKey;
                //options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(authenticationProviderKey, cfg =>
            {
                cfg.RequireHttpsMetadata = false;
                cfg.SaveToken = true;
                cfg.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = domain,
                    ValidAudience = domain,
                    ValidateAudience = true,
                    ValidateIssuer = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("3d0a6b32ed788db9f5598c1188b8f6d0")),
                    ClockSkew = TimeSpan.Zero // remove delay of token when expire
                };
            });
            services.AddScoped<IDapper, Dapperr>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "QLCD API By IOIT");
            });

            app.UseRouting();

            //app.UseCors("AllowCors");
            app.UseCors("AllowAll"); // add cros for all 
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            env.ContentRootPath = ContentRootPath;
        }
    }
}
