﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SalesWebMvc.Models
{
  public class Seller
  {
    public int Id { get; set; }

    [Required(ErrorMessage = "{0} required")] // Torna o campo "Nome" obrigatório
    [StringLength(60, MinimumLength = 3, ErrorMessage = "{0} size should be between {2} and {1}")]
    public string Name { get; set; }

    [Required(ErrorMessage = "{0} required")] // Torna o campo "E-mail" obrigatório
    [Display(Name = "E-mail")] // Altera o que aparece no cabeçalho da view
    [DataType(DataType.EmailAddress)] // Transforma os e-mails em Links para Enviar e-mail
    [EmailAddress(ErrorMessage = "Enter a valid email")] // Validação?
    public string Email { get; set; }

    [Required(ErrorMessage = "{0} required")] // Torna o campo "BirthDate" obrigatório
    [Display(Name = "Birth Date")] // Altera o que aparece no cabeçalho da view
    [DataType(DataType.Date)] // Formato da data no input
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")] // Formato da data
    public DateTime BirthDate { get; set; }

    [Required(ErrorMessage = "{0} required")] // Torna o campo "BaseSalary" obrigatório
    [Display(Name = "Base Salary")] // Altera o que aparece no cabeçalho da view
    [DisplayFormat(DataFormatString = "U$ {0:F2}")] // Formato do dinheiro
    public double BaseSalary { get; set; }
    public Department Department { get; set; } = new Department();
    public int DepartmentId { get; set; }
    public ICollection<SalesRecord> Sales { get; set; } = new HashSet<SalesRecord>();

    public Seller()
    {
    }

    public Seller(int id, string name, string email, DateTime birthDate, double baseSalary, Department department)
    {
      Id = id;
      Name = name;
      Email = email;
      BirthDate = birthDate;
      BaseSalary = baseSalary;
      Department = department;
    }

    public void AddSales(SalesRecord sr)
    {
      Sales.Add(sr);
    }

    public void RemoveSales(SalesRecord sr)
    {
      Sales.Remove(sr);
    }

    public double TotalSales(DateTime initial, DateTime final)
    {
      return Sales.Where(sr => initial <= sr.Date && sr.Date <= final).Sum(sr => sr.Amount); // Incrível!!
    }
  }
}
