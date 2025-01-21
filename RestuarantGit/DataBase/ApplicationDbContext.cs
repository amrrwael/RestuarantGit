using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Delivery.Resutruant.API.Models.Domain;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace Delivery.Resutruant.API.DataBase
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

     

   
    }
}
