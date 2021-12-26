using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interface;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class MessagesController : BaseApiController
    {
        private readonly IUserRepository _userRepository;
        private readonly IMessageRepository _messageRepository;
        private readonly IMapper _mapper;

        public MessagesController(IUserRepository userRepository, IMessageRepository messageRepository,
                                                    IMapper mapper) 
        {
            _messageRepository = messageRepository;
            _mapper = mapper;
            _userRepository = userRepository;
        }

        [HttpPost]
        public async Task<ActionResult<MessageDto>>  CreateMessage(CreateMessageDto createMessageDto)
        {
          var username = User.GetUsername();
          if(username == createMessageDto.RecipientUsername.ToLower())
                return BadRequest("You cannot send message to yourself");

            var sender = await _userRepository.GetUserByUserNameAsync(username);
            var recipient = await _userRepository.GetUserByUserNameAsync(createMessageDto.RecipientUsername);

            if(recipient == null) return NotFound();

            var message = new Message
            {
                Sender = sender,
                Recipient = recipient,
                SenderUsername = sender.UserName,
                RecipientUsername = recipient.UserName,
                Content = createMessageDto.Content
            };

             _messageRepository.AddMessasge(message);

             if(await _messageRepository.SaveAllAsync()) 
                    return Ok(_mapper.Map<MessageDto>(message));

            return BadRequest("Failed to send message");
        }

        [HttpGet("thread/{username}")]
        public async Task<ActionResult<IEnumerable<MessageDto>>> GetMessageThread(string username)
        {
            var currentUsername = User.GetUsername();

            return Ok(await  _messageRepository.GetMessasgeThread(currentUsername, username));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MessageDto>>> GetMessagesForUser([FromQuery]
            MessageParams messageParams)
            {
                messageParams.Username = User.GetUsername();
                
                var messages = await _messageRepository.GetMessasgeForUser(messageParams);

                Response.AddPaginationHeader(messages.CurrentPage, messages.PageSize, 
                                    messages.TotalCount,messages.TotalPages);
                return messages;
            }
        
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteMessage(int id)
        {
            var username = User.GetUsername();
            var message = await _messageRepository.GetMessasge(id);

            if(message.Sender.UserName!= username && message.Recipient.UserName!=username)
                return Unauthorized();
            
            if(message.Sender.UserName == username) message.SenderDeleted = true;
            
            if(message.Recipient.UserName == username) message.RecipientDeleted = true;

            if(message.SenderDeleted && message.RecipientDeleted)
                _messageRepository.DeleteMessasge(message);

            if(await _messageRepository.SaveAllAsync()) return Ok();

            return BadRequest("Problem Deleting the message");


        }
    }
}