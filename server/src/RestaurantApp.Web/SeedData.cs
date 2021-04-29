using RestaurantApp.Core.Interface;
using RestaurantApp.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantApp.Web
{
    public class SeedData
    {
        private readonly ApplicationDbContext db;
        private readonly ILoggerAdapter<SeedData> logger;

        public SeedData(ApplicationDbContext db, ILoggerAdapter<SeedData> logger)
        {
            this.db = db;
            this.logger = logger;
        }

        public void DataSeed()
        {
            
        }
    }
}
