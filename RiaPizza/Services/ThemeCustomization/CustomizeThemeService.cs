﻿using Microsoft.EntityFrameworkCore;
using RiaPizza.Data;
using RiaPizza.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RiaPizza.Services.ThemeCustomization
{
    public interface ICustomizeThemeService
    {
        Task<CustomizeTheme> GetThemeElements();
        Task UpdateLogo(string file);
    }
    public class CustomizeThemeService : ICustomizeThemeService
    {

        private readonly AppDbContext _context;
        public CustomizeThemeService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<CustomizeTheme> GetThemeElements()
        {
            var theme = await _context.CustomizeTheme.FirstOrDefaultAsync();
            return theme;
        }

        public async Task UpdateLogo(string file)
        {
            var theme = await _context.CustomizeTheme.FirstOrDefaultAsync();
            if(theme == null)
            {
                theme = new CustomizeTheme
                {
                    Logo = file   
                };
                await _context.CustomizeTheme.AddAsync(theme);
                await _context.SaveChangesAsync();
            }
            theme.Logo = file;
            _context.CustomizeTheme.Update(theme);
            await _context.SaveChangesAsync();
        }
    }
}
