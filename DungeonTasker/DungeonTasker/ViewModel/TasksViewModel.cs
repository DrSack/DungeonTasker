﻿using DungeonTasker.Models;
using DungeonTasker.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xamarin.Forms;

namespace DungeonTasker.ViewModel
{
    class TasksViewModel
    {
        public string Character { get; set; }
        public string Tasks { get; set; }
        public INavigation Navigation;
        public UserModel Currentuser;
        public Command AddTime { get; set; }
        /*
         * Contructor for DetailsPage to encapsulate current user information and truth value
         * 
         * PARAM
         * user: parse the user to be used within this class
         * truth: parse truth to notify Device.StartTimer to stop whenever truthtime is off
         * 
         * RETURN Nothing
         */
        public TasksViewModel(UserModel user, TasksView page)
        {
            this.Currentuser = user;
            Character = Currentuser.Character;
            Tasks = "Current Tasks";
            AddTime = new Command(async () => await this.Navigation.PushModalAsync(new DatePickerView(page, false)));
            // Name.Text = Currentuser.Username;
        }
    }
  }

