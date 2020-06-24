﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.ComponentModel;
using WindEditor.View;
using WindEditor.ViewModel;
using WindEditor.Events;
using NodeNetwork.Views;
using NodeNetwork.ViewModels;
using NodeNetwork.Toolkit.Layout.ForceDirected;

namespace WindEditor.Editor.Modes
{
    public class EventMode : IEditorMode
    {
        private DockPanel m_ModeControlsDock;
        private ComboBox m_EventCombo;

        private WDetailsViewViewModel m_EventDetailsViewModel;
        private WDetailsViewViewModel m_ActorDetailsViewModel;

        private EventNodeWindow m_NodeWindow;

        private WEventList m_EventList;
        private Event m_SelectedEvent;
        private Staff m_SelectedStaff;

        public WDetailsViewViewModel EventDetailsViewModel
        {
            get { return m_EventDetailsViewModel; }
            set
            {
                if (value != m_EventDetailsViewModel)
                {
                    m_EventDetailsViewModel = value;
                    OnPropertyChanged("EventDetailsViewModel");
                }
            }
        }

        public WDetailsViewViewModel ActorDetailsViewModel
        {
            get { return m_ActorDetailsViewModel; }
            set
            {
                if (value != m_ActorDetailsViewModel)
                {
                    m_ActorDetailsViewModel = value;
                    OnPropertyChanged("ActorDetailsViewModel");
                }
            }
        }

        public DockPanel ModeControlsDock
        {
            get { return m_ModeControlsDock; }
            set
            {
                if (value != m_ModeControlsDock)
                {
                    m_ModeControlsDock = value;
                    OnPropertyChanged("ModeControlsDock");
                }
            }
        }

        public WEventList EventList
        {
            get { return m_EventList; }
            set
            {
                if (value != m_EventList)
                {
                    m_EventList = value;
                    OnPropertyChanged("EventList");
                }
            }
        }

        public Event SelectedEvent
        {
            get { return m_SelectedEvent; }
            set
            {
                if (value != m_SelectedEvent)
                {
                    m_SelectedEvent = value;
                    OnPropertyChanged("SelectedEvent");
                }
            }
        }

        public Staff SelectedStaff
        {
            get { return m_SelectedStaff; }
            set
            {
                if (value != m_SelectedStaff)
                {
                    m_SelectedStaff = value;
                    OnPropertyChanged("SelectedStaff");
                }
            }
        }

        public WWorld World { get; }

        public event EventHandler<GenerateUndoEventArgs> GenerateUndoEvent;

        public EventMode(WWorld world)
        {
            World = world;
            EventDetailsViewModel = new WDetailsViewViewModel();
            ActorDetailsViewModel = new WDetailsViewViewModel();

            ModeControlsDock = CreateUI();

            m_NodeWindow = new EventNodeWindow();
            m_NodeWindow.ActorPropertiesView.DataContext = ActorDetailsViewModel;
            m_NodeWindow.ActorTabControl.SelectionChanged += OnSelectedActorChanged;
            m_NodeWindow.Closing += M_NodeWindow_Closing;
        }

        private void M_NodeWindow_Closing(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
        }

        /// <summary>
        /// Generates a DockPanel that contains the controls specific to this editor mode.
        /// </summary>
        private DockPanel CreateUI()
        {
            DockPanel event_dock_panel = new DockPanel()
            {
                LastChildFill = true
            };

            StackPanel event_stack_panel = new StackPanel()
            {
                HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch,
                VerticalAlignment = System.Windows.VerticalAlignment.Stretch,
            };

            RowDefinition combo_row = new RowDefinition();
            RowDefinition properties_row = new RowDefinition();

            combo_row.Height = System.Windows.GridLength.Auto;
            combo_row.MaxHeight = 500;
            combo_row.MinHeight = 10;
            properties_row.Height = System.Windows.GridLength.Auto;
            //properties_row.MinHeight = 80;

            Grid ev_grid = new Grid();
            ev_grid.RowDefinitions.Add(combo_row);
            ev_grid.RowDefinitions.Add(properties_row);

            m_EventCombo = new ComboBox()
            {
                HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch,
                DisplayMemberPath = "Name"
            };

            m_EventCombo.SelectionChanged += OnEventSelectionChanged;

            Grid.SetRow(m_EventCombo, 0);

            WDetailsView actor_details = new WDetailsView()
            {
                Name = "Details",
                HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch,
                VerticalAlignment = System.Windows.VerticalAlignment.Stretch,
                DataContext = EventDetailsViewModel
            };

            GroupBox actor_prop_box = new GroupBox()
            {
                HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch,
                VerticalAlignment = System.Windows.VerticalAlignment.Stretch,
                Header = "Properties",
                Content = actor_details,
            };

            Grid.SetRow(actor_prop_box, 1);

            ev_grid.Children.Add(m_EventCombo);
            ev_grid.Children.Add(actor_prop_box);

            DockPanel.SetDock(event_stack_panel, Dock.Top);

            event_stack_panel.Children.Add(ev_grid);
            event_dock_panel.Children.Add(event_stack_panel);

            return event_dock_panel;
        }

        private void OnEventSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (m_EventCombo.SelectedIndex == -1)
            {
                SelectedEvent = m_EventList.Events[0];
            }
            else
            {
                SelectedEvent = EventList.Events[m_EventCombo.SelectedIndex];
            }

            m_EventDetailsViewModel.ReflectObject(SelectedEvent);

            m_NodeWindow.ActorTabControl.Items.Clear();

            foreach (Staff s in SelectedEvent.Actors)
            {
                m_NodeWindow.ActorTabControl.Items.Add(GenerateActorTabItem(s));
            }

            m_NodeWindow.ActorTabControl.SelectedIndex = 0;
        }

        private void OnSelectedActorChanged(object sender, SelectionChangedEventArgs e)
        {
            if (m_NodeWindow.ActorTabControl.SelectedIndex == -1)
            {
                SelectedStaff = null;
            }
            else
            {
                SelectedStaff = SelectedEvent.Actors[m_NodeWindow.ActorTabControl.SelectedIndex];
            }

            ActorDetailsViewModel.ReflectObject(SelectedStaff);
        }

        public void BroadcastUndoEventGenerated(WUndoCommand command)
        {
            //throw new NotImplementedException();
        }

        public void ClearSelection()
        {
            //throw new NotImplementedException();
        }

        public void FilterSceneForRenderer(WSceneView view, WWorld world)
        {
            foreach (WScene scene in world.Map.SceneList)
            {
                foreach (var renderable in scene.GetChildrenOfType<IRenderable>())
                    renderable.AddToRenderer(view);
            }
        }

        public void OnBecomeActive()
        {
            WStage stage = (WStage)World.Map.SceneList.First(x => x.GetType() == typeof(WStage));
            EventList = stage.GetChildrenOfType<WEventList>()[0];

            m_EventCombo.ItemsSource = EventList.Events;
            m_EventCombo.SelectedIndex = 0;

            m_NodeWindow.Show();
        }

        public void OnBecomeInactive()
        {
            m_NodeWindow.Hide();
        }

        public void Update(WSceneView view)
        {
            //throw new NotImplementedException();
        }

        private TabItem GenerateActorTabItem(Staff staff)
        {
            // Create view model for node network.
            NetworkViewModel model = new NetworkViewModel();

            // Create the begin node for the actor and add it to the network.
            NodeViewModel begin_node = new NodeViewModel() { Name = staff.Name };
            model.Nodes.Edit(x => x.Add(begin_node));

            // Add an output to the begin node labelled "Begin".
            NodeOutputViewModel begin_output = new NodeOutputViewModel() { Name = "Begin", Port = new ExecPortViewModel { PortType = PortType.Execution } };
            begin_node.Outputs.Edit(x => x.Add(begin_output));

            // If this actor has at least one cut...
            if (staff.FirstCut != null)
            {
                int x_offset = 400;

                System.Windows.Point node_location = begin_node.Position;
                node_location.X += x_offset - begin_node.Size.Width;

                // Add the first cut to the node network.
                model.Nodes.Edit(x => x.Add(staff.FirstCut.NodeViewModel));

                // Create a connection between the output of the begin node and the input of the first cut.
                ConnectionViewModel first_to_begin = new ConnectionViewModel(
                    model,
                    staff.FirstCut.NodeViewModel.Inputs.Items.First(),
                    begin_output);
                // Add the connection to the node network.
                model.Connections.Edit(x => x.Add(first_to_begin));

                // Iterating through the linked list of cuts...
                Cut c = staff.FirstCut;
                while (c.NextCut != null)
                {
                    // Offset the position of the node so they're not on top of each other
                    c.NodeViewModel.Position = node_location;
                    node_location.X += c.NodeViewModel.Size.Width + x_offset;

                    c.AddPropertiesToNode(model);

                    // Create a connection between this cut and the next cut in the list.
                    ConnectionViewModel next_cut_connection = new ConnectionViewModel(
                        model,
                        c.NextCut.NodeViewModel.Inputs.Items.First(),
                        c.NodeViewModel.Outputs.Items.First());

                    // Add the connection to the node network.
                    model.Connections.Edit(x => x.Add(next_cut_connection));

                    // Grab the next cut and add it to the node network.
                    c = c.NextCut;
                    model.Nodes.Edit(x => x.Add(c.NodeViewModel));
                }

                // Catch the last node, which didn't go through the while loop
                c.NodeViewModel.Position = node_location;
                node_location.X += c.NodeViewModel.Size.Width + x_offset;

                c.AddPropertiesToNode(model);
            }

            // Create the visual component of the node network.
            NetworkView v = new NetworkView()
            {
                HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch,
                VerticalAlignment = System.Windows.VerticalAlignment.Stretch,
                ViewModel = model
            };

            // Finally, create the new tab.
            TabItem new_tab = new TabItem() { Header = staff.Name, Content = v };
            return new_tab;
        }

        #region INotifyPropertyChanged Support

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region IDisposable Support

        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~CollisionMode()
        // {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }

        #endregion
    }
}