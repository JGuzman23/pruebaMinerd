using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MinerdPruebaTecnica.Data.Contracts;
using MinerdPruebaTecnica.Data.Repositories;
using MinerdPruebaTecnica.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MinerdPruebaTecnica.Controllers
{
  public class CustomerController : Controller
  {
    ICustomerRepository _customerRepository;
    public CustomerController(ICustomerRepository customerRepository)
    {
      _customerRepository = customerRepository;
    }
    [Authorize]
    public async Task<IActionResult> Index(string SearchText)
    {
      var customers = await _customerRepository.GetAllAsync();

      if (!string.IsNullOrEmpty(SearchText))
      {
        customers = customers.Where(i => i.Name.Contains(SearchText) || i.ID.ToString() == SearchText);
      }

      return View(customers);
    }

    public IActionResult Create()
    {
      return View();
    }
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Create(Customer model)
    {
      ViewData["ErrorMessage"] = null;
      model.CreatedAt = DateTime.Now;

      if (!ModelState.IsValid)
        return View(model);

      try
      {
        var result = await _customerRepository.CreateAsync(model);

        if (!result)
        {
          ViewData["ErrorMessage"] = "El cliente no pudo ser creado.";
          return View(model);
        }

        return RedirectToAction(nameof(Index));
      }
      catch (Exception ex)
      {
        ViewData["ErrorMessage"] = ex.Message;
        return View(model);
      }
    }
    [Authorize]
    public async Task<IActionResult> Details(int id)
    {
      if (id == 0)
        return NotFound();

      var customer = await _customerRepository.GetByIdAsync(id);

      if (customer == null)
        return NotFound();

      return View(customer);
    }

    public async Task<IActionResult> Edit(int id)
    {
      if (id == 0) return NotFound();

      var customer = await _customerRepository.GetByIdAsync(id);

      if (customer == null) return NotFound();

      return View(customer);
    }
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Edit(Customer model)
    {
      if (!ModelState.IsValid) return View(model);

      try
      {
        var customer = await _customerRepository.GetByIdAsync(model.ID);

        if (customer == null) return NotFound();

        customer.Name = model.Name;
        customer.Apellido = model.Apellido;
        customer.Cedula = model.Cedula;
        customer.Telefono = model.Telefono;

        var result = await _customerRepository.EditAsync(customer);

        if (!result)
        {
          ViewData["ErrorMessage"] = "El cliente no pudo ser editado";
          return View(model);
        }

        return RedirectToAction(nameof(Index));
      }
      catch (Exception ex)
      {
        ViewData["ErrorMessage"] = ex.Message;
        return View(model);
      }
    }
    [Authorize]
    public async Task<IActionResult> Delete(int id)
    {
      if (id == 0) return NotFound();

      var customer = await _customerRepository.GetByIdAsync(id);

      if (customer == null) return NotFound();

      return View(customer);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Delete(Customer model)
    {
      try
      {
        var customer = await _customerRepository.GetByIdAsync(model.ID);

        if (customer == null) return NotFound();

        var result = await _customerRepository.RemoveAsync(customer);

        if (!result)
        {
          ViewData["ErrorMessage"] = "El Cliente no pudo ser eliminado correctamente.";
          return View(model);
        }

        return RedirectToAction(nameof(Index));
      }
      catch (Exception ex)
      {
        ViewData["ErrorMessage"] = ex.Message;
        return View(model);
      }
    }
  }
}
