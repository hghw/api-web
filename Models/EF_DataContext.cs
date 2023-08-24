﻿using api_web.Module.MainPage.Models;
using Microsoft.EntityFrameworkCore;
using WebAPI;

public class EF_DataContext : DbContext
{
    public EF_DataContext(DbContextOptions<EF_DataContext> options) : base(options) { }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseSerialColumns();
    }
    public DbSet<Location> Locations
    {
        get;
        set;
    }   
    public DbSet<User> Users
    {
        get;
        set;
    }
}