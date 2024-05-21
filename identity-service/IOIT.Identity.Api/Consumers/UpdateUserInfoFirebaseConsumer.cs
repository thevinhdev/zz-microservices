using Google.Cloud.Firestore;
using IOIT.Identity.Api.Models;
using IOIT.Identity.Application.Towers.Commands.Update;
using IOIT.Identity.Application.Towers.Queries;
using IOIT.Identity.Application.TypeAttributeItems.Commands.Update;
using IOIT.Identity.Application.TypeAttributeItems.Queries;
using IOIT.Identity.Application.Users.Queries;
using IOIT.Shared.ViewModels.DtoQueues;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;
using static IOIT.Shared.Commons.Enum.AppEnum;

namespace IOIT.Identity.Api.Consumers
{
    public class UpdateUserInfoFirebaseConsumer : IConsumer<DtoUpdateInfoFirebaseQueue>
    {
        private readonly IMediator _mediator;
        private readonly ILogger<UpdateUserInfoFirebaseConsumer> _logger;
        private FirestoreDb fireStoreDb;
        private string projectId;

        public UpdateUserInfoFirebaseConsumer(IMediator mediator, ILogger<UpdateUserInfoFirebaseConsumer> logger)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _logger = logger;

            string fileName = "serviceAccountKey.json";
            string webRootPath = Directory.GetCurrentDirectory();
            string newPath = Path.Combine(webRootPath, fileName);
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", newPath);
            projectId = "manager-residential";
            fireStoreDb = FirestoreDb.Create(projectId);
        }

        public async Task Consume(ConsumeContext<DtoUpdateInfoFirebaseQueue> context)
        {
            var data = await _mediator.Send(new InfoUserQuery
            {
                id = context.Message.userId
            });

            if (data != null)
            {
                UserFS userFS = new UserFS();
                userFS.userId = data.UserId;
                userFS.fullname = data.FullName;
                userFS.avata = data.Avata;
                userFS.status = data.Status != null ? (int)data.Status : (int)EntityStatus.DELETED;
                userFS.countNotification = context.Message.countNotification;
                userFS.countSupportRequire = context.Message.countSupportRequire;

                DocumentReference empRef = fireStoreDb.Collection("users").Document(context.Message.userId.ToString());
                await empRef.SetAsync(userFS, SetOptions.Overwrite);

                await Task.CompletedTask;
            }

            _logger.LogInformation("Success");
        }
    }
}
