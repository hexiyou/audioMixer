﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using audioMixer.Model;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows.Input;

namespace audioMixer.ViewModel
{
    public class TracksViewModel
    {
        private TracksModel model;

        public ObservableCollection<TrackViewModel> TrackList { get; private set; }

        private void copyTracks()
        {
            TrackList.CollectionChanged -= modelSync;
            TrackList.Clear();
            foreach (TrackModel track in model)
            {
                TrackList.Add(new TrackViewModel(track));
            }
            TrackList.CollectionChanged += modelSync;
        }

        public TracksViewModel()
        {
            TrackList = new ObservableCollection<TrackViewModel>();
            model = new TracksModel();

            model.AddTrack(new TrackModel("Metalica - Nothing else matters"));
            model.AddTrack(new TrackModel("Rammstein - Du hast"));
            model.AddTrack(new TrackModel("Eminem - Lose yourself"));
            model.AddTrack(new TrackModel("AC/DC - TNT"));
            model.AddTrack(new TrackModel("Slipknot - Psychosocial"));

            copyTracks();
        }

        private void modelSync(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    TrackViewModel newTrack = (TrackViewModel)e.NewItems[0];
                    if (newTrack != null)
                    {
                        model.AddTrack(newTrack.getModel());
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    TrackViewModel deletedTrack = (TrackViewModel)e.OldItems[0];
                    if (deletedTrack != null)
                    {
                        model.RemoveTrack(deletedTrack.getModel());
                    }
                    break;
            }
        }

        private ICommand addTrack;
        public ICommand AddTrack
        {
            get
            {
                if (addTrack == null)
                {
                    addTrack = new RelayCommand(
                        o => 
                        {
                            TrackModel track = new TrackModel("Just added track");
                            TrackList.Add(new TrackViewModel(track));
                        }
                    );
                }
                return addTrack;
            }
        }

        private ICommand removeTrack;
        public ICommand RemoveTrack
        {
            get
            {
                if (removeTrack == null)
                {
                    removeTrack = new RelayCommand(
                        o =>
                        {
                            TrackList.Remove((TrackViewModel)o);
                        }
                    );
                }
                return removeTrack;
            }
        }
    }
}