using Microsoft.AspNetCore.Http;
using RestaurantApp.Core.Interface;
using RestaurantApp.Core.Model;
using RestaurantApp.Core.RepositoryInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace RestaurantApp.Core.Manager
{
    public abstract class FileManager<File> : IFileManager<File> where File : class
    {
        protected readonly ILoggerAdapter<File> logger;
        protected readonly IUnitOfWork unitOfWork;
        protected readonly string rootWebLocation;
        protected readonly string hostUrl;

        protected FileManager(IUnitOfWork unitOfWork, ILoggerAdapter<File> logger, string rootWebLocation, string hostUrl)
        {
            this.unitOfWork = unitOfWork;
            this.logger = logger;
            this.rootWebLocation = rootWebLocation;
            this.hostUrl = hostUrl;
        }

        public abstract OperationResult UploadFile(File dbEntity, IFormFile originalFile);
        public abstract OperationResult DeleteFile(int id);
        public abstract OperationResult DeleteFile(File dbEntity);
        public abstract IEnumerable<File> GetAllFiles(Expression<Func<File, bool>> predicate = null, Func<IQueryable<File>, IOrderedQueryable<File>> orderBy = null);
        public abstract File GetFileById(int id);
    }
}
