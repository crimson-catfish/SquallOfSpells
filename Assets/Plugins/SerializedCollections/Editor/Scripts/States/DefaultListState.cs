using UnityEditor;
using UnityEngine;

namespace AYellowpaper.SerializedCollections.Editor.States
{
    internal class DefaultListState : ListState
    {
        public DefaultListState(SerializedDictionaryInstanceDrawer serializedDictionaryDrawer) : base(
            serializedDictionaryDrawer)
        {
        }

        public override int ListSize => this.Drawer.ListProperty.minArraySize;

        public override void OnEnter()
        {
            this.Drawer.ReorderableList.draggable = true;
        }

        public override void OnExit()
        {
        }

        public override ListState OnUpdate()
        {
            if (this.Drawer.SearchText.Length > 0)
                return this.Drawer.SearchState;

            return this;
        }

        public override void DrawElement(Rect rect, SerializedProperty property, DisplayType displayType)
        {
            SerializedDictionaryInstanceDrawer.DrawElement(rect, property, displayType);
        }

        public override SerializedProperty GetPropertyAtIndex(int index)
        {
            return this.Drawer.ListProperty.GetArrayElementAtIndex(index);
        }

        public override void RemoveElementAt(int index)
        {
            this.Drawer.ListProperty.DeleteArrayElementAtIndex(index);
        }

        public override void InserElementAt(int index)
        {
            this.Drawer.ListProperty.InsertArrayElementAtIndex(index);
            this.Drawer.ListProperty.serializedObject.ApplyModifiedProperties();
        }
    }
}