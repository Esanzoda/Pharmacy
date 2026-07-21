using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Pharmasy.Models.Domain;

namespace Pharmasy.Data;

public class AuditableInterceptor : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        if (eventData.Context is not null)
        {
            var entityEntries = eventData.Context.ChangeTracker.Entries<BaseEntity>();

            foreach (var entityEntry in entityEntries)
            {
                if (entityEntry.State is EntityState.Added)
                {
                    entityEntry.Entity.CreatedAt = DateTime.UtcNow;
                    entityEntry.Entity.UpdateAt = DateTime.UtcNow;
                }

                if (entityEntry.State is EntityState.Modified)
                {
                    entityEntry.Entity.UpdateAt = DateTime.UtcNow;
                }

                if (entityEntry.State is EntityState.Deleted)
                {
                    entityEntry.Entity.IsDeleted = true;
                }
            }
        }

        return base.SavingChanges(eventData, result);
    }
}