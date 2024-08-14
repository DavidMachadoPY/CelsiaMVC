using Microsoft.EntityFrameworkCore;
using Celsia.Data;
using Celsia.Interfaces;
using Celsia.Models;
using Celsia.ViewModels;
using System;
using System.Threading.Tasks;

namespace Celsia.Services
{
    public class AuthRepository : IAuthRepository
    {
        private readonly BaseContext _context;

        public AuthRepository(BaseContext context)
        {
            _context = context;
        }

        public async Task<User> Login(UserLoginViewModel user)
        {
            // Busca el usuario por correo electrónico
            User? userFind = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == user.Email);

            // Si el usuario no existe, retorna null
            if (userFind == null)
            {
                return null!;
            }

            // Verifica la contraseña utilizando BCrypt
            if (!string.IsNullOrEmpty(user.Password) && BCrypt.Net.BCrypt.Verify(user.Password, userFind.Password))
            {
                return userFind;
            }

            // Si no coincide la contraseña, retorna null
            return null!;
        }

        public async Task<User> Register(UserCreateViewModel user)
        {
            // Verifica si el correo electrónico ya está en uso
            if (await _context.Users.AnyAsync(u => u.Email == user.Email))
            {
                throw new Exception("El correo ya está en uso.");
            }

            // Crea un nuevo usuario a partir del ViewModel
            User userRegistration = new User
            {
                Id = user.Id,
                Name = user.Name!,
                Email = user.Email!,
                Password = BCrypt.Net.BCrypt.HashPassword(user.Password), // Encripta la contraseña
                Address = user.Address!,
                Phone = user.Phone!,
                Status = "Active",
                CreateAt = DateTime.Now,
                UpdateAt = DateTime.Now
            };

            // Agrega el nuevo usuario a la base de datos y guarda los cambios
            await _context.Users.AddAsync(userRegistration);
            await _context.SaveChangesAsync();
            return userRegistration;
        }

        public async Task<User> UpdateUser(UserUpdateViewModel userUpdate, string userId)
        {
            // Busca al usuario en la base de datos
            var user = await _context.Users.FindAsync(userId);

            if (user == null)
            {
                throw new Exception("Usuario no encontrado.");
            }

            // Actualiza la información del usuario
            user.Name = userUpdate.Name!;
            user.Address = userUpdate.Address!;
            user.Phone = userUpdate.Phone!;
            user.Email = userUpdate.Email!;
            user.UpdateAt = DateTime.UtcNow;

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return user;
        }

        // Métodos adicionales como GetUserById y DeleteUser podrían ser añadidos aquí si se requieren en el futuro.
    }
}
