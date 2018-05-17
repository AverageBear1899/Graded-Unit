using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace GradedUnit.Models.Data
{
    /// <summary>
    /// This class is used as a way for each individual class to communicate with their associated tables
    /// </summary>
    public class Db : DbContext
    {
        /// <summary>
        /// Pages table
        /// </summary>
        public DbSet<PageDTO> Pages { get; set; }
        /// <summary>
        /// Sidebar table
        /// </summary>
        public DbSet<SidebarDTO> Sidebar { get; set; }
        /// <summary>
        /// Catagories table
        /// </summary>
        public DbSet<CategoryDTO> Catagories { get; set; }
        /// <summary>
        /// Products table
        /// </summary>
        public DbSet<ProductDTO> Products { get; set; }
        /// <summary>
        /// Users table
        /// </summary>
        public DbSet<UserDTO> Users { get; set; }
        /// <summary>
        /// Roles table
        /// </summary>
        public DbSet<RoleDTO> Roles { get; set; }
        /// <summary>
        /// Userroles table
        /// </summary>
        public DbSet<UserRoleDTO> UserRoles { get; set; }
        /// <summary>
        /// Orders table
        /// </summary>
        public DbSet<OrderDTO> Orders { get; set; }
        /// <summary>
        /// OrderDetails table
        /// </summary>
        public DbSet<OrderDetailsDTO> OrderDetails { get; set; }




    }
}