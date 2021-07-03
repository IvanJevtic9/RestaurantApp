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

        private string GetImageFolderLocation(Image dbEntity)
        {
            var filePurp = "";

            switch (dbEntity.Role)
            {
                case ImageRole.Profile:
                    filePurp = Constants.PROFILE_PICTURE_LOCATION;
                    break;
                case ImageRole.Menu:
                    filePurp = Constants.MENU_BANNER_LOCATION;
                    break;
                case ImageRole.MenuItem:
                    filePurp = Constants.MENU_ITEM_LOCATION;
                    break;
                case ImageRole.Gallery:
                    filePurp = Constants.GALLERY_IMAGE_LOCATION;
                    break;
            }

            return filePurp;
        }

        public override OperationResult DeleteFile(int id)
        {
            var image = unitOfWork.Image.GetById(id);

            return DeleteFile(image);
        }

        public override OperationResult DeleteFile(Image dbEntity)
        {
            var op = new OperationResult() { Succeeded = true };

            string filePurp = GetImageFolderLocation(dbEntity);

            var fileLocation = Path.Combine(rootWebLocation, filePurp, dbEntity.ImageName);
            File.Delete(fileLocation);

            unitOfWork.Image.Remove(dbEntity);
            unitOfWork.SaveChanges();
            logger.LogInformation($"File {dbEntity.ImageName} has been deleted.");

            return op;
        }

        public override OperationResult UploadFile(Image dbEntity, IFormFile originalFile)
        {
            var op = new OperationResult() { Succeeded = true };

            string filePurp = GetImageFolderLocation(dbEntity);

            var fileName = $"{DateTime.Now.ToString("ddMMyyyy_HH_mm_ss")}_{dbEntity.ImageName}";
            var fileLocation = Path.Combine(rootWebLocation, filePurp);
            var fileUri = Path.Combine(hostUrl, filePurp, fileName);

            dbEntity.ImageName = fileName;
            dbEntity.Url = fileUri;

            if (Directory.Exists(fileLocation))
            {
                using (var filesStream = new FileStream(Path.Combine(fileLocation, fileName), FileMode.Create))
                {
                    originalFile.CopyTo(filesStream);
                }

                unitOfWork.Image.Add(dbEntity);
                unitOfWork.SaveChanges();
                logger.LogInformation($"An Image {dbEntity.ImageName} has been uploaded.");
            }
            else
            {
                op.AddError("fileLocation", $"Internal error - location {fileLocation} does not exist.");
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
