using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProyectoIdentity.Models;

namespace ProyectoIdentity.Controllers
{
    public class CuentasController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;


        public CuentasController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager= userManager;
            _signInManager= signInManager;
        }


        public IActionResult Index()
        {
            return View();
        }

        [HttpGet] 
        public async Task<IActionResult> Registro() 
        {
            RegistroViewModel registroVM = new RegistroViewModel();
            return View(registroVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken] // Este Decorador se usa para prevenir ataques al enviar la informacion
        public async Task<IActionResult> Registro(RegistroViewModel rgViewModel)
        {
            if (ModelState.IsValid)
            {
                var usuario = new AppUsuario();
                usuario.UserName = rgViewModel.Email;
                usuario.Email = rgViewModel.Email;
                usuario.Nombre= rgViewModel.Nombre;
                usuario.Url = rgViewModel.Url;
                usuario.CodigoPais = rgViewModel.CodigoPais;
                usuario.Telefono = rgViewModel.Telefono;
                usuario.Pais = rgViewModel.Pais;
                usuario.Ciudad = rgViewModel.Ciudad;
                usuario.Direccion = rgViewModel.Direccion;
                usuario.FechaNacimiento = rgViewModel.FechaNacimiento;
                usuario.Estado = rgViewModel.Estado;
                //var usuario = new AppUsuario(UserName = rgViewModel.Email, Email = rgViewModel.Email, Url = rgViewModel.Url, CodigoPais = rgViewModel.CodigoPais, Telefono = rgViewModel.Telefono, Pais = rgViewModel.Pais, Ciudad = rgViewModel.Ciudad, Direccion = rgViewModel.Direccion, FechaNacimiento = rgViewModel.FechaNacimiento, Estado = rgViewModel.Estado);
                var resultado = await _userManager.CreateAsync(usuario, rgViewModel.Password);

                if(resultado.Succeeded)
                {
                    await _signInManager.SignInAsync(usuario, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }
                ValidarErrores(resultado);
            }

            return View(rgViewModel);
        }
        
        private void ValidarErrores(IdentityResult resultado)
        {
            foreach(var error in resultado.Errors) 
            {
                ModelState.AddModelError(String.Empty, error.Description);
            }
        }


        [HttpGet]
        public IActionResult Acceso()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Acceso(AccesoViewModel accViewModel)
        {
            if (ModelState.IsValid)
            {
                
                var resultado = await _signInManager.PasswordSignInAsync(accViewModel.Email, accViewModel.Password, accViewModel.RememberMe, lockoutOnFailure: false);

                if (resultado.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Acceso inválido");
                    return View(accViewModel);
                }
            }

            return View(accViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SalirAplicacion()
        {
            await _signInManager.SignOutAsync(); //Este SignOutAsync() cierra la sesion y borra las cookies del navegador 
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

    }
}
