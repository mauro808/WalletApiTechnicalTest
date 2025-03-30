using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WalletApi.Application.DTOs;
using WalletApi.Application.Interfaces;
using WalletAPI.Application.Services;
using WalletAPI.Domain.Entities;
using WalletAPI.Infrastructure.Persistence;
using Xunit;

namespace WalletAPI.Tests.WalletTest
{
    public class WalletTest
    {
        private readonly ApplicationDbContext _context;
        private readonly WalletService _walletService;

        public WalletTest()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDB")
            .Options;

            _context = new ApplicationDbContext(options);
            _walletService = new WalletService(_context);
        }

        [Fact]
        public async Task CreateWalletAsync_ShouldThrowException_WhenDocumentIdAlreadyExists()
        {
            // Arrange
            _context.Wallets.Add(new Wallet { Id = 1, DocumentId = "12345678", Name = "Usuario 1" });
            await _context.SaveChangesAsync();

            var newWalletDto = new Wallet { DocumentId = "12345678", Name = "Nuevo Usuario" };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _walletService.CreateWalletAsync(newWalletDto));
        }
    }
}
