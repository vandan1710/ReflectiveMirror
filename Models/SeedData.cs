using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using ReflectiveMirror.Models;
using ReflectiveMirror.Data;

namespace ReflectiveMirror.Models;

public static class SeedData
{
    public static void Initialize(IServiceProvider serviceProvider)
    {
        using (var context = new ReflectiveMirrorContext(
            serviceProvider.GetRequiredService<
                DbContextOptions<ReflectiveMirrorContext>>()))
        {
            if (context.Mirror.Any())
            {
                return;
            }

            context.Mirror.AddRange(
                new Mirror
                {
                    Height = 180,
                    Width = 90,
                    Shape = "Rectangular",
                    Price = 250,
                    Type = "Wall-mounted",
                    Material = "glass",
                    ImageUrl = ""
                },
                new Mirror
                {
                    Height = 120,
                    Width = 80,
                    Shape = "Oval",
                    Price = 300,
                    Type = "Vanity",
                    Material = "Beveled glass",
                    ImageUrl = ""
                },
                new Mirror
                {
                    Height = 75,
                    Width = 75,
                    Shape = "Round",
                    Price = 150,
                    Type = "Wall-mounted",
                    Material = "Bamboo Frame",
                    ImageUrl = ""
                },
                new Mirror
                {
                    Height = 60,
                    Width = 40,
                    Shape = "Rectangular",
                    Price = 75,
                    Type = "Bathroom",
                    Material = "Anti-Fog",
                    ImageUrl = ""
                },
                new Mirror
                {
                    Height = 150,
                    Width = 40,
                    Shape = "Rectangular",
                    Price = 100,
                    Type = "Full -length",
                    Material = "Metal Frame",
                    ImageUrl = ""
                },
                new Mirror
                {
                    Height = 170,
                    Width = 60,
                    Shape = "Arch",
                    Price = 180,
                    Type = "Full-Length",
                    Material = "Wood Frame",
                    ImageUrl = ""
                },
                new Mirror
                {
                    Height = 50,
                    Width = 30,
                    Shape = "Rectangular",
                    Price = 125,
                    Type = "Bathroom",
                    Material = "LED Backlit",
                    ImageUrl = ""
                },
                new Mirror
                {
                    Height = 100,
                    Width = 100,
                    Shape = "Square",
                    Price = 200,
                    Type = "Wall-mounted",
                    Material = "Rattan Frame",
                    ImageUrl = ""
                },

                new Mirror
                {
                    Height = 40,
                    Width = 40,
                    Shape = "Round",
                    Price = 50,
                    Type = "Portable",
                    Material = "Acrylic",
                    ImageUrl = ""
                },
                new Mirror
                {
                    Height = 30,
                    Width = 80,
                    Shape = "Rectangular",
                    Price = 140,
                    Type = "Bathroom",
                    Material = "Integrated Shelf",
                    ImageUrl = ""
                }             
                );
            context.SaveChanges();
        }
    }
}
