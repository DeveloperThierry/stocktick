using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Stock;
using api.Helpers;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
    public class StockRepository : IStockRepository
    {
        private readonly ApplicationDBContext _context;

        public StockRepository(ApplicationDBContext context)
        {
            _context = context;
        }
       
        public async Task<List<Stock>> GetAllAsync(QueryObject query)
        {
            var stocks = _context.Stocks.Include(c => c.Comments).AsQueryable();
            if(!string.IsNullOrWhiteSpace(query.CompanyName))
            {
                stocks = stocks.Where(s => s.CompanyName != null && s.CompanyName.ToLower().Contains(query.CompanyName.ToLower().Trim()));
            }
             if(!string.IsNullOrWhiteSpace(query.Symbol))
            {
                stocks = stocks.Where(s => s.Symbol != null && s.Symbol.ToLower().Contains(query.Symbol.ToLower().Trim()));
            }
             if(!string.IsNullOrWhiteSpace(query.SortBy))
            {
                if(query.SortBy.Equals("Symbol", StringComparison.OrdinalIgnoreCase))
                {
                    stocks = query.IsDescending ? stocks.OrderByDescending(s => s.Symbol) : stocks.OrderBy(s => s.Symbol);
                }
            }

            var SkipNumber = (query.PageNumber - 1) * query.PageSize;

            return await stocks.Skip(SkipNumber).Take(query.PageSize).ToListAsync();
        }
        public async Task<Stock?> GetByIdAsync(int id)
        {
            var stock = await _context.Stocks.Include(c => c.Comments).FirstOrDefaultAsync(i => i.Id == id);
            if(stock == null)
            {
                return null;
            }

            return stock;
        }
        public async Task<Stock> CreateAsync(Stock stockModel)
        {
            await _context.Stocks.AddAsync(stockModel);
            await _context.SaveChangesAsync();
            return stockModel;
        }
        public async Task<Stock?> UpdateAsync(int id, UpdateStockRequestDto updateDto)
        {
            var stockModel = await _context.Stocks.FirstOrDefaultAsync(x => x.Id == id);
            if(stockModel == null)
            {
                return null;
            }
            stockModel.Symbol = updateDto.Symbol;
            stockModel.CompanyName = updateDto.CompanyName;
            stockModel.Purchase = updateDto.Purchase;
            stockModel.LastDiv = updateDto.LastDiv;
            stockModel.Industry = updateDto.Industry;
            stockModel.MarketCap = updateDto.MarketCap;
            await _context.SaveChangesAsync();
            return stockModel;
        }
        public async Task<Stock?> PartialUpdateAsync(int id, PartialUpdateStockRequestDto updateDto)
        {
            var stockModel = await _context.Stocks.FirstOrDefaultAsync(x => x.Id == id);
            if(stockModel == null)
            {
                return null;
            }
            if(updateDto.Symbol != null) stockModel.Symbol = updateDto.Symbol;
            if(updateDto.CompanyName != null) stockModel.CompanyName = updateDto.CompanyName;
            if(updateDto.Purchase.HasValue) stockModel.Purchase = updateDto.Purchase.Value;
            if(updateDto.LastDiv.HasValue) stockModel.LastDiv = updateDto.LastDiv.Value;
            if(updateDto.Industry != null) stockModel.Industry = updateDto.Industry;
            if(updateDto.MarketCap.HasValue) stockModel.MarketCap = updateDto.MarketCap.Value;
            await _context.SaveChangesAsync();
            return stockModel;
        }
        public async Task<Stock?> DeleteAsync(int id)
        {
            var stockModel = await _context.Stocks.FirstOrDefaultAsync(x => x.Id == id);
            if(stockModel == null) {
                return null;
            }
            _context.Stocks.Remove(stockModel);
            await _context.SaveChangesAsync();
            return stockModel;
        }

        public async Task<bool> StockExists(int id)
        {
            return await _context.Stocks.AnyAsync(c => c.Id == id);
        }

        public async Task<Stock> GetBySymbolAsync(string symbol)
        {
            return await _context.Stocks.FirstOrDefaultAsync(s => s.Symbol == symbol);
        }
    }
}