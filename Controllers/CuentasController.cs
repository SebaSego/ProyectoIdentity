using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Identity.UI.V4.Pages.Account.Internal;
using Microsoft.AspNetCore.Mvc;
using ProyectoIdentity.Models;

namespace ProyectoIdentity.Controllers
{
    public class CuentasController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IEmailSender _emailSender;


        public CuentasController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IEmailSender emailSender)
        {
            _userManager= userManager;
            _signInManager= signInManager;
            _emailSender= emailSender;
        }


        public IActionResult Index()
        {
            return View();
        }

        [HttpGet] 
        public async Task<IActionResult> Registro(string returnurl = null) 
        {
            ViewData["ReturnUrl"] = returnurl;
            RegistroViewModel registroVM = new RegistroViewModel();
            return View(registroVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken] // Este Decorador se usa para prevenir ataques al enviar la informacion
        public async Task<IActionResult> Registro(RegistroViewModel rgViewModel, string returnurl = null)
        {
            ViewData["ReturnUrl"] = returnurl;
            returnurl = returnurl ?? Url.Content("~/");

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
                    //return RedirectToAction("Index", "Home");
                    return LocalRedirect(returnurl);

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
        public IActionResult Acceso(string returnurl=null )
        {
            ViewData["ReturnUrl"]= returnurl;
            returnurl = returnurl??Url.Content("~/");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Acceso(AccesoViewModel accViewModel, string returnurl = null)
        {
            ViewData["ReturnUrl"] = returnurl;
            returnurl = returnurl??Url.Content("~/");

            if (ModelState.IsValid)
            {
                
                var resultado = await _signInManager.PasswordSignInAsync(accViewModel.Email, accViewModel.Password, accViewModel.RememberMe, lockoutOnFailure: true);

                if (resultado.Succeeded)
                {
                    //return RedirectToAction("Index", "Home");
                    return LocalRedirect(returnurl);
                }
                if (resultado.IsLockedOut)
                {
                    return View("Bloqueado");

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


        //Metodo para Recuperar contraseña
        [HttpGet]
        public IActionResult OlvidoPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OlvidoPassword(OlvidoPasswordViewModel opViewModel)
        {
            if (ModelState.IsValid)
            {
                var usuario = await _userManager.FindByEmailAsync(opViewModel.Email);
                if (usuario == null)
                {
                    return RedirectToAction("ConfirmacionOlvidoPassword");
                }
                var codigo = await _userManager.GeneratePasswordResetTokenAsync(usuario);
                var urlRetorno = Url.Action("ResetPassword", "Cuentas", new {userId = usuario.Id, code = codigo}, protocol:HttpContext.Request.Scheme);

                await _emailSender.SendEmailAsync(opViewModel.Email, "Recuperar Contraseña - Proyecto Identity", "Recupere su contraseña haciendo click aqui: <a href=\""+ urlRetorno + "\">enlace</a>");

                return RedirectToAction("ConfirmacionOlvidoPassword");


            }
            return View(opViewModel);           
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ConfirmacionOlvidoPassword()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ResetPassword(string code=null)
        {
            return code == null? View("Error ") : View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(RecuperaPasswordViewModel rpViewModel)
        {
            if (ModelState.IsValid)
            {
                var usuario = await _userManager.FindByEmailAsync(rpViewModel.Email);
                if (usuario == null)
                {
                    return RedirectToAction("ConfirmacionRecuperaPassword");
                }
                
                var resultado = await _userManager.ResetPasswordAsync(usuario, rpViewModel.Code, rpViewModel.Password);
                
                if (resultado.Succeeded)
                {
                    return RedirectToAction("ConfirmacionRecuperaPassword");

                }

                ValidarErrores(resultado);


            }
            return View(rpViewModel);
        }

        [HttpGet]
        public IActionResult ConfirmacionRecuperaPassword() 
        {
            return View();
        }

    }
}
