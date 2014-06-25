using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Chowlog.Entities;

namespace Chowlog.Web.DataContexts
{
    public class ChowlogDb : DbContext
    {
        public ChowlogDb() : base("DefaultConnection")
        {

        }
        public DbSet<Plate> Plates { get; set; }
    }
}