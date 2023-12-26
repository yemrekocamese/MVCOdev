using Microsoft.AspNetCore.Mvc;
using MVCHomework.Data;
using MVCHomework.Models;
using MVCHomework.Utils;

namespace MVCHomework.Controllers;

public class AuthController : Controller
{
    private readonly Context _context;

    public AuthController(Context context)
    {
        _context = context;
    }

    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Login(User model)
    {
        if (ModelState.IsValid)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == model.Email && u.Password == model.Password);

            if (user != null)
            {
                SetUserSession(user);
                return RedirectToAction("Index", "Home");
            }
        }

        // Eğer hata varsa, hataları görüntüleyin ve kullanıcıyı tekrar Login sayfasına yönlendirin.
        return View(model);
    }

    public IActionResult Register()
    {
        return View();
    }


    [HttpPost]
    public IActionResult Register(User model)
    {
        if (ModelState.IsValid)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == model.Email);

            if (user == null)
            {
                Models.User userToAdd = new User();

                userToAdd.Email = model.Email;
                userToAdd.Password = model.Password;

                _context.Users.Add(userToAdd);
                _context.SaveChanges();

                _context.UserRoles.Add(new UserRole()
                {
                    UserId = userToAdd.Id,
                    RoleId = 2 // basic user role 
                });
                _context.SaveChanges();

                SetUserSession(userToAdd);

                GlobalVariables.Context = HttpContext;
                return RedirectToAction("Index", "Home");
            }
        }

        // Eğer hata varsa, hataları görüntüleyin ve kullanıcıyı tekrar Register sayfasına yönlendirin.
        return View(model);
    }


    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Index", "Home");
    }

    private void SetUserSession(User user)
    {
        var userRole = _context.UserRoles.Where(r => r.UserId == user.Id).Select(r => r.Role.Name).ToString();

        HttpContext.Session.SetString("User", user.Email);
        HttpContext.Session.SetString("Role", userRole);
    }
}