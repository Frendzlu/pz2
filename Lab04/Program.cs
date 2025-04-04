﻿// See https://aka.ms/new-console-template for more information


using System.Security.Cryptography;
using Lab04;

var territories = Utils.ReadCsvFile<Territory>("../../../data/territories.csv");
var employees = Utils.ReadCsvFile<Employee>("../../../data/employees.csv");
var employeeTerritories = Utils.ReadCsvFile<EmployeeTerritory>("../../../data/employee_territories.csv");
var regions = Utils.ReadCsvFile<Region>("../../../data/regions.csv");
var orders = Utils.ReadCsvFile<Order>("../../../data/orders.csv");
var orderDetails = Utils.ReadCsvFile<OrderDetails>("../../../data/orders_details.csv");
// Console.WriteLine("Territories length: " + territories.Count);
// Console.WriteLine("Employees length: " + employees.Count);
// Console.WriteLine("EmpTer length: " + employeeTerritories.Count);
// Console.WriteLine("Regions length: " + regions.Count);

var employeeSurnames = employees.Select(e => e.Surname).ToList();

Utils.PrintList(employeeSurnames);

var employeeRegionTerritories = from employee in employees
    join employeeTerritory in employeeTerritories on employee.Id equals employeeTerritory.EmployeeId
    join territory in territories on employeeTerritory.TerritoryId equals territory.Id
    join region in regions on territory.RegionId equals region.Id
    select new 
    {
        EmployeeSurname = employee.Surname,
        RegionName = region.Name,
        TerritoryName = territory.Name
    };

var employeeRegionTerritoryList = employeeRegionTerritories.ToList();

Utils.PrintList(employeeRegionTerritoryList);

var regionEmployeeSurnames = from region in regions
    join territory in territories on region.Id equals territory.RegionId
    join employeeTerritory in employeeTerritories on territory.Id equals employeeTerritory.TerritoryId
    join employee in employees on employeeTerritory.EmployeeId equals employee.Id
    group employee.Surname by region.Name into regionGroup
    select new
    {
        RegionName = regionGroup.Key,
        EmployeeSurnames = string.Join(", ", regionGroup.Distinct())
    };

var regionEmployeeSurnamesList = regionEmployeeSurnames.ToList();

Utils.PrintList(regionEmployeeSurnamesList);

var regionEmployeeCount = from region in regions
    join territory in territories on region.Id equals territory.RegionId
    join employeeTerritory in employeeTerritories on territory.Id equals employeeTerritory.TerritoryId
    join employee in employees on employeeTerritory.EmployeeId equals employee.Id
    group employee by region.Name into regionGroup
    select new
    {
        RegionName = regionGroup.Key,
        EmployeeCount = regionGroup.Distinct().Count()
    };

var regionEmployeeCountList = regionEmployeeCount.ToList();

Utils.PrintList(regionEmployeeCountList);

var orderWithTotal = from order in orders
    join orderDetail in orderDetails on order.OrderId equals orderDetail.OrderId
    group orderDetail by order
    into orderGroup
    select new
    {
        Order = orderGroup.Key,
        TotalPrice = orderGroup.Sum(od => od.UnitPrice * od.Quantity * (1 - od.Discount))
    };

var eoc = from od in orderWithTotal
    join employee in employees on od.Order.EmployeeId equals employee.Id
    group od by new
    {
        FullName = employee.ToString()
    }
    into god
    select new
    {
        Employee = god.Key.FullName,
        NumOfOrders = god.Count(),
        AvgOrderValue = god.Average(od => od.TotalPrice),
        MaxOrderValue = god.Max(od => od.TotalPrice),
    };


var employeeOrderCountList = eoc.ToList();

Utils.PrintList(employeeOrderCountList);
