﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProiectMDS.DAL;
using ProiectMDS.DAL.Entities.Auth;
using ProiectMDS.DAL.Models.UserModels;
using ProiectMDS.Services.UriServicess;
using ProiectSoft.BLL.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils.Wrappers;
using Utils.Wrappers.Filters;

namespace ProiectMDS.Services.UserServices
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IUriServices _uriServices;

        public UserService(AppDbContext context, 
            IMapper mapper,
            IUriServices uriServices)
        {
            _context = context;
            _mapper = mapper;   
            _uriServices = uriServices;
        }

        public async Task Delete(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);

            if (user == null)
            {
                throw new KeyNotFoundException($"Thre is no user with id:{id}");
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }

        public async Task<PagedResponse<List<UserGetModel>>> GetAll(UserFilter filter, string route)
        {          
            IQueryable<User> users = _context.Users;

            if (!String.IsNullOrEmpty(filter.searchName))
            {
                users = users.Where(x => x.UserName!.Contains(filter.searchName));
            }

            if (!string.IsNullOrEmpty(filter.userName))
            {
                users = users.Where(x => x.UserName!.Contains(filter.userName));
            }

            if (!string.IsNullOrEmpty(filter.firstName))
            {
                users = users.Where(x => x.FirstName!.Contains(filter.firstName));
            }

            if (filter.dateCreated != null)
            {
                users = users.Where(x => x.DateCreated.ToString("mm/dd/yyyy").Contains(filter.dateCreated.Value.ToString("mm/dd/yyyy")));
            }

            switch (filter.orderBy)
            {
                case "Name":
                    users = !filter.descending == false ? users.OrderBy(x => x.UserName) : users.OrderByDescending(x => x.UserName);
                    break;
                case "Email":
                    users = !filter.descending == false ? users.OrderBy(s => s.Email) : users.OrderByDescending(x => x.Email);
                    break;
                case "Age":
                    users = !filter.descending == false ? users.OrderBy(s => s.FirstName) : users.OrderByDescending(x => x.FirstName);
                    break;
                default:
                    users = !filter.descending == false ? users.OrderBy(s => s.LastName) : users.OrderByDescending(x => x.LastName);
                    break;
            }

            var userModels = users
                .Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .Select(_mapper.Map<UserGetModel>)
                .ToList();

            var usersListCount = await _context.Users.CountAsync();

            var pagedResponse = PaginationHelper.CreatePagedReponse<UserGetModel>(userModels, filter, usersListCount, _uriServices, route);

            return pagedResponse;
        }

        public async Task<Response<UserGetModel>> GetById(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);

            if (user == null)
            {
                throw new KeyNotFoundException($"Thre is no user with id: {id}");
            }

            var userGetModel = _mapper.Map<UserGetModel>(user);

            return new Response<UserGetModel>(userGetModel);
        }

        public async Task Update(UserPutModel model, int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);

            if (user == null)
            {
                throw new KeyNotFoundException($"There is no user with id:{id}");
            }

            _mapper.Map<UserPutModel, User>(model, user);
            user.DateModified = DateTime.Now;

            await _context.SaveChangesAsync();
        }
    }
}
