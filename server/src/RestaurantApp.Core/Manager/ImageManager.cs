using Microsoft.AspNetCore.Http;
using RestaurantApp.Core.Entity;
using RestaurantApp.Core.Interface;
using RestaurantApp.Core.Model;
using RestaurantApp.Core.RepositoryInterface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace RestaurantApp.Core.Manager
{
    public class ImageManager : FileManager<Image>
    {
        public ImageManager(IUnitOfWork unitOfWork, ILoggerAdapter<Image> logger, string rootWebLocation, string hostUrl) :
            base(unitOfWork, logger, rootWebLocation, hostUrl)
        { }

        public override OperationResult DeleteFile(int id)
        {
            var image = unitOfWork.Image.GetById(id);

            return DeleteFile(image);
        }

        public override OperationResult DeleteFile(Image dbEntity)
        {
            var op = new OperationResult() { Succeeded = true };

            string filePurp = "";
            switch (dbEntity.Role)
            {
                case ImageRole.Profile:
                    filePurp = Constants.PROFILE_PICTURE_LOCATION;
                    break;
            }

            var fileLocation = Path.Combine(rootWebLocation, filePurp, dbEntity.ImangeName);
            File.Delete(fileLocation);

            unitOfWork.Image.Remove(dbEntity);
            unitOfWork.SaveChanges();
            logger.LogInformation($"File {dbEntity.ImangeName} has been deleted.");

            return op;
        }

        public override OperationResult UploadFile(Image dbEntity, IFormFile originalFile)
        {
            var op = new OperationResult() { Succeeded = true };

            string filePurp = "";
            switch (dbEntity.Role)
            {
                case ImageRole.Profile:
                    filePurp = Constants.PROFILE_PICTURE_LOCATION;
                    break;
            }

            var fileName = $"{DateTime.Now.ToString("ddMMyyyy_HH_mm_ss")}_{dbEntity.ImangeName}";
            var fileLocation = Path.Combine(rootWebLocation, filePurp);
            var fileUri = Path.Combine(hostUrl, filePurp, fileName);

            dbEntity.ImangeName = fileName;
            dbEntity.Url = fileUri;

            if (Directory.Exists(fileLocation))
            {
                using (var filesStream = new FileStream(Path.Combine(fileLocation, fileName), FileMode.Create))
                {
                    originalFile.CopyTo(filesStream);
                }

                unitOfWork.Image.Add(dbEntity);
                unitOfWork.SaveChanges();
                logger.LogInformation($"An Image {dbEntity.ImangeName} has been uploaded.");
            }
            else
            {
                op.AddError("fileLocation", "Internal error - fileLocation does not exist.");
                op.Succeeded = false;

                logger.LogError(new InvalidOperationException(), "File location does not exist.");
                return op;
            }

            return op;
        }

        public override IEnumerable<Image> GetAllFiles(Expression<Func<Image, bool>> predicate = null, Func<IQueryable<Image>, IOrderedQueryable<Image>> orderBy = null)
        {
            return unitOfWork.Image.GetAll(predicate, orderBy);
        }

        public override Image GetFileById(int id)
        {
            return unitOfWork.Image.GetById(id);
        }
    }
}
