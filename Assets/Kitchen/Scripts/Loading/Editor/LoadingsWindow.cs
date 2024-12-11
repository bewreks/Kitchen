using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Kitchen.Scripts.Generated;
using UniRx;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Kitchen.Scripts.Loading.Editor
{
    public class LoadingsWindow : EditorWindow
    {
        [SerializeField] protected VisualTreeAsset uxmlAsset;
        
        private List<LoadingItem> _items;
        private MultiColumnListView _multiColumnListView;
        private IDisposable _disposable;
        
        [MenuItem("Window/Loading monitor")]
        static void Summon()
        {
            GetWindow<LoadingsWindow>("Loading monitor");
        }

        void CreateGUI()
        {
            uxmlAsset.CloneTree(rootVisualElement);

            rootVisualElement.Q<Button>("Instantiate").visible = false;
            rootVisualElement.Q<Button>("Release").visible = false;
            rootVisualElement.Q<Button>("Clear").visible = false;

            _multiColumnListView = rootVisualElement.Q<MultiColumnListView>();

            _multiColumnListView.showBoundCollectionSize = false;
            _multiColumnListView.virtualizationMethod = CollectionVirtualizationMethod.DynamicHeight;
            
            _multiColumnListView.selectionType = SelectionType.Single;
            
            UpdateItems();

            _multiColumnListView.columns["Key"].makeCell = () =>
            {
                var label = new Label();
                label.SetEnabled(false);
                return label;
            };
            _multiColumnListView.columns["Status"].makeCell = () =>
            {
                var field = new Label();
                field.SetEnabled(false);
                return field;
            };
            _multiColumnListView.columns["Progress"].makeCell = () =>
            {
                var field = new FloatField();
                field.SetEnabled(false);
                return field;
            };
            _multiColumnListView.columns["Count"].makeCell = () =>
            {
                var field = new IntegerField();
                field.SetEnabled(false);
                return field;
            };
            
            _multiColumnListView.columns["Key"].bindCell = (VisualElement element, int index) =>
                (element as Label).text = _items[index].Key;
            _multiColumnListView.columns["Status"].bindCell = (VisualElement element, int index) =>
                (element as Label).text = _items[index].Handle.Status.ToString();
            _multiColumnListView.columns["Progress"].bindCell = (VisualElement element, int index) =>
                (element as FloatField).value = _items[index].Handle.PercentComplete;
            _multiColumnListView.columns["Count"].bindCell = (VisualElement element, int index) =>
                (element as IntegerField).value = _items[index].InstantiationCount;

            _disposable?.Dispose();
            _disposable = Observable.Timer(TimeSpan.FromSeconds(Time.fixedDeltaTime)).Subscribe(_ => UpdateItems());
        }

        private void OnGUI()
        {
            _disposable ??= Observable.Timer(TimeSpan.FromSeconds(Time.fixedDeltaTime)).Subscribe(_ => UpdateItems());
        }

        private void OnDestroy()
        {
            _disposable?.Dispose();
            _disposable = null;
        }

        private void UpdateItems()
        {
            _items = LoadingManager.LoadingOperations.Values.ToList();
            _multiColumnListView.itemsSource = _items;
            _multiColumnListView.RefreshItems();
        }
    }
}
