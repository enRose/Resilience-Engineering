﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using api.Helpers;

namespace api.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20211005210657_RemovedUserTable")]
    partial class RemovedUserTable
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.10");

            modelBuilder.Entity("retry.Entities.Content", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<int?>("PersonalLoanId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("PersonalLoanId");

                    b.ToTable("Content");
                });

            modelBuilder.Entity("retry.Entities.PersonalLoan", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<long>("UserId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("PersonalLoan");
                });

            modelBuilder.Entity("retry.Entities.Content", b =>
                {
                    b.HasOne("retry.Entities.PersonalLoan", null)
                        .WithMany("App")
                        .HasForeignKey("PersonalLoanId");
                });

            modelBuilder.Entity("retry.Entities.PersonalLoan", b =>
                {
                    b.Navigation("App");
                });
#pragma warning restore 612, 618
        }
    }
}
