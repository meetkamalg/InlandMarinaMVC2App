using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InlandMarinaData
{
    public static class InlandMarinaManager
    {
        // gets all avalible slips
        public static List<Slip> GetSlips(InlandMarinaContext db)
        {
            return db.Slips
                .Where(s => !s.Leases.Any())
                .ToList();
        }
        // gets the distinct dock ids for the filter dropdown list
        public static List<Dock> GetDocks(InlandMarinaContext db)
        {
            return db.Docks
            .Select(d => d.ID)
            .Distinct()
            .Select(dockId => new Dock { ID = dockId })
            .ToList();

        }
        // gets all avalible slips for the selected dock id
        public static List<Slip> GetSlipsByDocks(InlandMarinaContext db, int dockId)
        {
            return db.Slips
                .Where(s => s.DockID == dockId && s.Leases.Count == 0)
                .ToList();
        }
        // gets all slips leased by a customer
        public static List<Slip> GetLeasedSlips(int? customerid)
        {
            using (InlandMarinaContext db = new InlandMarinaContext())
            {
                List<Lease> leases = db.Leases.Include(l => l.Slip).Where(l => l.CustomerID == customerid).ToList();
                List<Slip> leasedSlips = leases.Select(l => l.Slip).ToList();

                return leasedSlips;

            }
        }
    }
}
