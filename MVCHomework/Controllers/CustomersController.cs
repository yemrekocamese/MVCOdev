﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVCHomework.Data;
using MVCHomework.Models;

namespace MVCHomework.Controllers
{
    public class CustomersController : Controller
    {
        private readonly Context _context;

        public CustomersController(Context context)
        {
            _context = context;
        }

        // GET: Customers
        public async Task<IActionResult> Index()
        {
            var context = _context.Customers.Include(c => c.Address).Include(c => c.CustomerType).Include(c => c.Payment).Include(c => c.User);
            return View(await context.ToListAsync());
        }

        // GET: Customers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Customers == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .Include(c => c.Address)
                .Include(c => c.CustomerType)
                .Include(c => c.Payment)
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // GET: Customers/Create
        public IActionResult Create()
        {
            ViewData["AddressId"] = new SelectList(_context.Addresses, "Id", "Id");
            ViewData["CustomerTypeId"] = new SelectList(_context.CustomerTypes, "Id", "Id");
            ViewData["PaymentId"] = new SelectList(_context.Payments, "Id", "Id");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,AddressId,CustomerTypeId,UserId,PaymentId")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                _context.Add(customer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AddressId"] = new SelectList(_context.Addresses, "Id", "Id", customer.AddressId);
            ViewData["CustomerTypeId"] = new SelectList(_context.CustomerTypes, "Id", "Id", customer.CustomerTypeId);
            ViewData["PaymentId"] = new SelectList(_context.Payments, "Id", "Id", customer.PaymentId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", customer.UserId);
            return View(customer);
        }

        // GET: Customers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Customers == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            ViewData["AddressId"] = new SelectList(_context.Addresses, "Id", "Id", customer.AddressId);
            ViewData["CustomerTypeId"] = new SelectList(_context.CustomerTypes, "Id", "Id", customer.CustomerTypeId);
            ViewData["PaymentId"] = new SelectList(_context.Payments, "Id", "Id", customer.PaymentId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", customer.UserId);
            return View(customer);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,AddressId,CustomerTypeId,UserId,PaymentId")] Customer customer)
        {
            if (id != customer.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(customer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExists(customer.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["AddressId"] = new SelectList(_context.Addresses, "Id", "Id", customer.AddressId);
            ViewData["CustomerTypeId"] = new SelectList(_context.CustomerTypes, "Id", "Id", customer.CustomerTypeId);
            ViewData["PaymentId"] = new SelectList(_context.Payments, "Id", "Id", customer.PaymentId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", customer.UserId);
            return View(customer);
        }

        // GET: Customers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Customers == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .Include(c => c.Address)
                .Include(c => c.CustomerType)
                .Include(c => c.Payment)
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Customers == null)
            {
                return Problem("Entity set 'Context.Customers'  is null.");
            }
            var customer = await _context.Customers.FindAsync(id);
            if (customer != null)
            {
                _context.Customers.Remove(customer);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CustomerExists(int id)
        {
          return (_context.Customers?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
