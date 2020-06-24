﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using WindEditor.Properties;
using GameFormatReader.Common;

namespace WindEditor.Events
{
    public enum StaffType
    {
        Normal,
        All,
        Camera,
        New_Name_3,
        Timekeeper,
        New_Name_5,
        Director,
        Message,
        Sounds,
        Light,
        New_Name_4,
        Package,
        Create
    }

    [HideCategories()]
    public class Staff : INotifyPropertyChanged
    {
        private string m_Name;

        private int m_DuplicateID;
        private int m_Flag;
        private StaffType m_StaffType;
        private int m_FirstCutIndex;

        private Cut m_FirstCut;

        public Cut FirstCut
        {
            get { return m_FirstCut; }
            set
            {
                if (value != m_FirstCut)
                {
                    m_FirstCut = value;
                    OnPropertyChanged("FirstCut");
                }
            }
        }

        [WProperty("Staff", "Name", true, "Name of the actor.")]
        public string Name
        {
            get { return m_Name; }
            set
            {
                if (value != m_Name)
                {
                    m_Name = value;
                    OnPropertyChanged("Name");
                }
            }
        }

        [WProperty("Staff", "Staff Type", true, "Identifies special handling of this staff. Probably do NOT change this manually.")]
        public StaffType StaffType
        {
            get { return m_StaffType; }
            set
            {
                if (value != m_StaffType)
                {
                    m_StaffType = value;
                    OnPropertyChanged("StaffType");
                }
            }
        }

        public Staff()
        {
            m_Name = "new_staff";

            m_Flag = 0;
            m_StaffType = StaffType.Normal;
            m_FirstCutIndex = -1;

            m_FirstCut = null;
        }

        public Staff(EndianBinaryReader reader)
        {
            Name = new string(reader.ReadChars(32)).Trim('\0');
            m_DuplicateID = reader.ReadInt32();

            reader.SkipInt32();

            m_Flag = reader.ReadInt32();
            m_StaffType = (StaffType)reader.ReadInt32();
            m_FirstCutIndex = reader.ReadInt32();

            reader.Skip(28);
        }

        public void AssignFirstCut(List<Cut> cut_list)
        {
            if (m_FirstCutIndex != -1)
            {
                FirstCut = cut_list[m_FirstCutIndex];
            }
        }

        public override string ToString()
        {
            string cut_name = FirstCut != null ? FirstCut.Name : "null";
            return $"Name: \"{ Name }\", First Cut: { cut_name }";
        }

        #region INotifyPropertyChanged Support

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}