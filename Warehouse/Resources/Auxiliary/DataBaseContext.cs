﻿//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Configuration;
//using Npgsql;

//namespace Warehouse.Resources.Auxiliary
//{
//    class DataBaseContext : DbContext
//    {
        
//        public DbSet<BaseElement> baseElements { get; set; }
//        public string token;

//        public DataBaseContext()
//        {
//            LoadToken();
//        }

//        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
//            optionsBuilder.UseNpgsql(token);
        
//        private async void LoadToken()
//        {
//            this.token = await SecureStorage.GetAsync("ConnectionDefault");
//        }
//        //public DataBaseContext(DbContextOptions<DataBaseContext> options) : base(options) {  }

//        //public DbSet<baseElement> Elements { get; set; }

//    }
//}
