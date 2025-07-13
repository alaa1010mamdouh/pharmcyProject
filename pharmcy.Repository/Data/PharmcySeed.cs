using Microsoft.EntityFrameworkCore;
using Pharmcy.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace pharmcy.Repository.Data
{
    public static class PharmcySeed
    {

        public static async Task SeedData(PharmcyContext context)
        {
            // 1. Seed Users
            if (!context.users.Any())
            {
                var usersData = await File.ReadAllTextAsync("../pharmcy.Repository/Data/DataSeed/User.json");
                var users = JsonSerializer.Deserialize<List<User>>(usersData);
                if (users?.Count > 0)
                {
                    foreach (var user in users)
                    {
                        if (string.IsNullOrEmpty(user.Name))
                        {
                            Console.WriteLine($"Warning: User with ID {user.Id} has empty Name, skipping...");
                            continue;
                        }

                        if (!context.users.Any(u => u.Id == user.Id))
                        {
                            await context.users.AddAsync(user);
                        }
                    }
                    await context.SaveChangesAsync();
                }
            }

            // 2. Seed Customers
            if (!context.customers.Any())
            {
                var customersData = await File.ReadAllTextAsync("../pharmcy.Repository/Data/DataSeed/Customers.json");
                var customers = JsonSerializer.Deserialize<List<Customer>>(customersData);
                if (customers?.Count > 0)
                {
                    foreach (var customer in customers)
                    {
                        if (!context.customers.Any(c => c.Id == customer.Id))
                        {
                            await context.customers.AddAsync(customer);
                        }
                    }
                    await context.SaveChangesAsync();
                }
            }

            //// 3. Seed Medicines
            if (!context.medicines.Any())
            {
                var medicinesData = await File.ReadAllTextAsync("../pharmcy.Repository/Data/DataSeed/Medicines.json");
                var medicines = JsonSerializer.Deserialize<List<Medicine>>(medicinesData);
                if (medicines?.Count > 0)
                {
                    foreach (var medicine in medicines)
                    {
                        if (!context.medicines.Any(m => m.Id == medicine.Id))
                        {
                            await context.medicines.AddAsync(medicine);
                        }
                    }
                    await context.SaveChangesAsync();
                }
            }

            //// 4. Seed Prescriptions (يجب أن يكون بعد Users و Customers)
            if (!context.prescriptions.Any())
            {
                var prescriptionsData = await File.ReadAllTextAsync("../pharmcy.Repository/Data/DataSeed/Prescriptions.json");
                var prescriptions = JsonSerializer.Deserialize<List<Prescription>>(prescriptionsData);
                if (prescriptions?.Count > 0)
                {
                    foreach (var prescription in prescriptions)
                    {
                        // التحقق من وجود Customer و User المرتبطين
                        bool customerExists = await context.customers.AnyAsync(c => c.Id == prescription.CustomerId);
                        bool userExists = await context.users.AnyAsync(u => u.Id == prescription.UserId);

                        if (customerExists && userExists)
                        {
                            if (!context.prescriptions.Any(p => p.Id == prescription.Id))
                            {
                                await context.prescriptions.AddAsync(prescription);
                            }
                        }
                        else
                        {
                            Console.WriteLine($"Warning: Prescription {prescription.Id} references missing customer ({prescription.CustomerId}) or user ({prescription.UserId}), skipping...");
                        }
                    }
                    await context.SaveChangesAsync();
                }
            }

            //// 5. Seed PrescriptionItems (يجب أن يكون بعد Prescriptions و Medicines)
            if (!context.prescriptionItems.Any())
            {
                var prescriptionItemsData = await File.ReadAllTextAsync("../pharmcy.Repository/Data/DataSeed/PrescriptionItems.json");
                var prescriptionItems = JsonSerializer.Deserialize<List<PrescriptionItem>>(prescriptionItemsData);
                if (prescriptionItems?.Count > 0)
                {
                    foreach (var item in prescriptionItems)
                    {
                        // التحقق من وجود Prescription و Medicine المرتبطين
                        bool prescriptionExists = await context.prescriptions.AnyAsync(p => p.Id == item.PrescriptionId);
                        bool medicineExists = await context.medicines.AnyAsync(m => m.Id == item.MedicineId);

                        if (prescriptionExists && medicineExists)
                        {
                            if (!context.prescriptionItems.Any(pi => pi.Id == item.Id))
                            {
                                await context.prescriptionItems.AddAsync(item);
                            }
                        }
                        else
                        {
                            Console.WriteLine($"Warning: PrescriptionItem {item.Id} references missing prescription ({item.PrescriptionId}) or medicine ({item.MedicineId}), skipping...");
                        }
                    }
                    await context.SaveChangesAsync();
                }
            }

            //// 6. Seed Invoices (يجب أن يكون بعد Customers)
            if (!context.invoices.Any())
            {
                var invoicesData = await File.ReadAllTextAsync("../pharmcy.Repository/Data/DataSeed/Invoices.json");
                var invoices = JsonSerializer.Deserialize<List<Invoice>>(invoicesData);
                if (invoices?.Count > 0)
                {
                    foreach (var invoice in invoices)
                    {
                        // التحقق من وجود Customer المرتبط
                        bool customerExists = await context.customers.AnyAsync(c => c.Id == invoice.CustomerId);

                        if (customerExists)
                        {
                            if (!context.invoices.Any(i => i.Id == invoice.Id))
                            {
                                await context.invoices.AddAsync(invoice);
                            }
                        }
                        else
                        {
                            Console.WriteLine($"Warning: Invoice {invoice.Id} references missing customer ({invoice.CustomerId}), skipping...");
                        }
                    }
                    await context.SaveChangesAsync();
                }
            }

            // 7. Seed InvoiceItems (يجب أن يكون بعد Invoices و Medicines)
            if (!context.invoiceItems.Any())
            {
                var invoiceItemsData = await File.ReadAllTextAsync("../pharmcy.Repository/Data/DataSeed/InvoiceItems.json");
                var invoiceItems = JsonSerializer.Deserialize<List<InvoiceItem>>(invoiceItemsData);
                if (invoiceItems?.Count > 0)
                {
                    foreach (var item in invoiceItems)
                    {
                        // التحقق من وجود Invoice و Medicine المرتبطين
                        bool invoiceExists = await context.invoices.AnyAsync(i => i.Id == item.InvoiceId);
                        bool medicineExists = await context.medicines.AnyAsync(m => m.Id == item.MedicineId);

                        if (invoiceExists && medicineExists)
                        {
                            if (!context.invoiceItems.Any(ii => ii.Id == item.Id))
                            {
                                await context.invoiceItems.AddAsync(item);
                            }
                        }
                        else
                        {
                            Console.WriteLine($"Warning: InvoiceItem {item.Id} references missing invoice ({item.InvoiceId}) or medicine ({item.MedicineId}), skipping...");
                        }
                    }
                    await context.SaveChangesAsync();
                }
            }

            // 8. Seed Reports (يجب أن يكون بعد Users)
            if (!context.reports.Any())
            {
                var reportsData = await File.ReadAllTextAsync("../pharmcy.Repository/Data/DataSeed/Reports.json"); // تصحيح مسار الملف
                var reports = JsonSerializer.Deserialize<List<Report>>(reportsData);
                if (reports?.Count > 0)
                {
                    foreach (var report in reports)
                    {
                        // التحقق من وجود User المرتبط
                        bool userExists = await context.users.AnyAsync(u => u.Id == report.UserId);

                        if (userExists)
                        {
                            if (!context.reports.Any(r => r.Id == report.Id))
                            {
                                await context.reports.AddAsync(report);
                            }
                        }
                        else
                        {
                            Console.WriteLine($"Warning: Report {report.Id} references missing user ({report.UserId}), skipping...");
                        }
                    }
                    await context.SaveChangesAsync();
                }
            }
        }




























        //public static async Task SeedData(PharmcyContext context)
        //{
        //    if (!context.users.Any())
        //    {


        //        var usersData = File.ReadAllText("../pharmcy.Repository/Data/DataSeed/User.json");
        //        var users = JsonSerializer.Deserialize<List<User>>(usersData);
        //        if (users?.Count > 0)
        //        {
        //            foreach (var user in users)
        //            {
        //                if (string.IsNullOrEmpty(user.Name))
        //                {
        //                    Console.WriteLine($"Warning: User with ID {user.Id} has empty Name, skipping...");
        //                    continue;
        //                }

        //                if (!context.users.Any(u => u.Id == user.Id))
        //                {
        //                    await context.users.AddAsync(user);
        //                }
        //            }
        //            await context.SaveChangesAsync();
        //        }



        //}
        //    /////////////////////////////////////////////////////////////////

        //    if (!context.customers.Any())
        //    {
        //        var customersData = await File.ReadAllTextAsync("../pharmcy.Repository/Data/DataSeed/Customers.json");
        //        var customers = JsonSerializer.Deserialize<List<Customer>>(customersData);
        //        if (customers?.Count > 0)
        //        {
        //            foreach (var customer in customers)
        //            {
        //                await context.Set<Customer>().AddAsync(customer);
        //            }
        //            await context.SaveChangesAsync();
        //        }
        //    }
        //    ////////////////////////////////////////////////////////
        //    if (!context.medicines.Any())
        //    {
        //        var medicinesData = await File.ReadAllTextAsync("../pharmcy.Repository/Data/DataSeed/Medicines.json");
        //        var medicines = JsonSerializer.Deserialize<List<Medicine>>(medicinesData);
        //        if (medicines?.Count > 0)
        //        {
        //            foreach (var medicine in medicines)
        //            {
        //                await context.Set<Medicine>().AddAsync(medicine);
        //            }
        //            await context.SaveChangesAsync();
        //        }
        //    }
        //    //////////////////////////////////////////////////////////////
        //    if (!context.prescriptions.Any())
        //    {
        //        var prescriptionsData = await File.ReadAllTextAsync("../pharmcy.Repository/Data/DataSeed/Prescriptions.json");
        //        var prescriptions = JsonSerializer.Deserialize<List<Prescription>>(prescriptionsData);
        //        if (prescriptions?.Count > 0)
        //        {
        //            foreach (var prescription in prescriptions)
        //            {

        //                await context.Set<Prescription>().AddAsync(prescription);

        //            }
        //            await context.SaveChangesAsync();
        //        }
        //    }


        //    if (!context.prescriptionItems.Any())
        //    {
        //        var prescriptionItemsData = await File.ReadAllTextAsync("../pharmcy.Repository/Data/DataSeed/PrescriptionItems.json");
        //        var prescriptionItems = JsonSerializer.Deserialize<List<PrescriptionItem>>(prescriptionItemsData);
        //        if (prescriptionItems?.Count > 0)
        //        {
        //            foreach (var item in prescriptionItems)
        //            {

        //                await context.Set<PrescriptionItem>().AddAsync(item);

        //            }
        //            await context.SaveChangesAsync();
        //        }
        //    }








        //    //////////////////////////////////////////////////////////////////////
        //    if (!context.invoices.Any())
        //    {
        //        var invoicesData = await File.ReadAllTextAsync("../pharmcy.Repository/Data/DataSeed/Invoices.json");
        //        var invoices = JsonSerializer.Deserialize<List<Invoice>>(invoicesData);
        //        if (invoices?.Count > 0)
        //        {
        //            foreach (var invoice in invoices)
        //            {
        //                await context.Set<Invoice>().AddAsync(invoice);

        //            }
        //            await context.SaveChangesAsync();
        //        }
        //    }

        //    //////////////////////////////////////////////////////////////////////////////////////
        //    if (!context.invoiceItems.Any())
        //    {
        //        var invoiceItemsData = await File.ReadAllTextAsync("../pharmcy.Repository/Data/DataSeed/InvoiceItems.json");
        //        var invoiceItems = JsonSerializer.Deserialize<List<InvoiceItem>>(invoiceItemsData);
        //        if (invoiceItems?.Count > 0)
        //        {
        //            foreach (var item in invoiceItems)
        //            {

        //                await context.Set<InvoiceItem>().AddAsync(item);

        //            }
        //            await context.SaveChangesAsync();
        //        }
        //    }
        //    //////////////////////////////////////////////////////////////////////////////////////
        //    if (!context.reports.Any())
        //    {


        //        var reportsData = await File.ReadAllTextAsync("../pharmcy.Repository/Data/DataSeed/PrescriptionItems.json");
        //        var reports = JsonSerializer.Deserialize<List<Report>>(reportsData);
        //        if (reports?.Count > 0)
        //        {
        //            foreach (var report in reports)
        //            {

        //                await context.Set<Report>().AddAsync(report);

        //            }
        //            await context.SaveChangesAsync();
        //        }
        //    }




    }
    }

