using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirfoilOptimizationTool.GAConfigurationWindow.AirfoilRepresentation {
    public class BasisAirfoilConfigManager {
        public event NotifyCollectionChangedEventHandler basisAirfoilItemsDidChange;
        public event EventHandler readyStatusDidChange;

        public bool isReady { get; private set; }
        public ObservableCollection<BasisAirfoilItem> basisAirfoilItems { get; }



        public BasisAirfoilConfigManager() {
            basisAirfoilItems = new ObservableCollection<BasisAirfoilItem>();

            basisAirfoilItems.CollectionChanged += BasisAirfoilItems_CollectionChanged;
        }

        //
        // Event Callbacks
        //
        private void BasisAirfoilItems_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e) {
            basisAirfoilItemsDidChange?.Invoke(sender, e);

            // Check whether the basisAirfoilItems are fulfill the condition of basisAirfoilMethod requirements.
            if (isReady != (basisAirfoilItems.Count != 0)) {
                isReady = basisAirfoilItems.Count != 0;
                readyStatusDidChange(this, new EventArgs());
            }
        }
    }
}
