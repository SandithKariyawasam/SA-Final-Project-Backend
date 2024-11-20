using AutoBid.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace AutoBid.Data
{
    public class DataContext : IdentityDbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }
        public DbSet<Auction> Auctions { get; set; }

        public DbSet<Biding> Bids { get; set; }

        public DbSet<payments> Payments { get; set; }
    }
}
