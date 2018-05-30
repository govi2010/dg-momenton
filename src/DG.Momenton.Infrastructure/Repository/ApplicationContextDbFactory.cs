using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Text;

namespace DG.Momenton.Infrastructure.Repository
{
    #region ApplicationContextDbFactory

    /// <summary>
    /// This class manages the generate-migration-script process via providing the data-context's options
    /// This will be used when we run the "Add-Migration" from the package-manager-console
    /// </summary>
    public class ApplicationContextDbFactory : IDesignTimeDbContextFactory<DataContext>
    {
        DataContext IDesignTimeDbContextFactory<DataContext>.CreateDbContext(string[] args)
        {
            // Set the default database
            var optionsBuilder = new DbContextOptionsBuilder<DataContext>();
            optionsBuilder.UseSqlServer<DataContext>(@"Server=.\\SQLExpress;Database=DgMomenton;Integrated Security=True;Trusted_Connection=True;MultipleActiveResultSets=true");

            // Return the data-context
            return new DataContext(optionsBuilder.Options);
        }
    }

    #endregion
}
