   M S B A M L   `   `   `  ���� I  ELiveViewPlugin, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null;"clr-namespace:LiveViewPlugin.UtilsLiveViewPlugin.Utils  W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089   TextBlockStyle  �� 
TargetType �� Property �� Value  TreeViewItemFocusVisual  ExpandCollapseToggleStyle �� 
TargetType J� SnapsToDevicePixels ��Name �� Triggers P� Property P� Value	 �� 
TargetName  TreeViewItemStyle
 �� Mode �� RelativeSource �� AncestorType/    )LiveViewPlugin.Utils.TreeViewItemBehavior � ColumnDefinitions � RowDefinitions d�Name �� 	ClickMode d� 	IsChecked ��Name �� Padding ��Name ��Name ]� 
Conditions �� Property �� Value �� Triggers  EmployeeDateTemplate �� Orientation � Color � Offset  DepartmentTemplate �� ItemTemplate �� ItemsSource�� D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    :x,http://schemas.microsoft.com/winfx/2006/xaml     0	localutil"clr-namespace:LiveViewPlugin.Utils  %�  &	        &	 �     &	 �     &	 �    7  7  &	 ~    &	 0    7  �� "  ��.���  � �� �#FFF��  � ��$ Verdana)���  � ��$
 Bold"���  � ��$ 14(���  � ��$ Center=���  � ��$ Center=��� .���  � �� �� .R��� �� "  d�.���  � |� .  ��  � ��$ 17����  � ��$ 17����  � �� �� " d�.R��� $��17��$��17��
������ ._��� $��17��$��17�� . 
������ ._�� .+��� $ closedfolder��$ ��../Images/folderclosed.png���� $ 
openfolder��$��../Images/openfolder.png�� P�  �@d�	IsChecked$
 TrueY�. ��� $	 closedfolder�� � y�$ 	Collapsed=�P�  �@d�	IsChecked$ FalseY�. ��� $	 
openfolder�� � y�$ 	Collapsed=��� "  Q�.���  �@Q�
IsExpanded �� *
IsExpanded+$
 TwoWay=���  � ��
 ���� ��  � �� �� *HorizontalContentAlignment+ �� " ����  � �� �� *VerticalContentAlignment+ �� " ����  � ��$ 1,0,0,0q���  � ��# � ����  �@�FocusVisualStyle8   �� % �@  IsBroughtIntoViewWhenSelected . ��  � �� �� " Q�.R��  �� $��19��$
��Auto ��� $
��Auto ��� $��* � �� $
��Auto ��� .+�d� $ Expander��$ Press=�
������ 8��  �� *
IsExpanded+ �� *TemplatedParent+�� $ Bd��#��z��#��z��#��z��$��1��# z�� . ._��� $ PART_Header��$��Header��#��z��#��z��# z �� $ 	ItemsHost��$��2��$��1��$��1�� P�  �@Q�
IsExpanded .  . ��� $	 	ItemsHost�� � y�$ 	Collapsed=�P�  �@��HasItems .  . ��� $	 Expander�� � y�$ Hidden=�P�  �@Q�
IsSelected . . ��� $	 Bd�� � ��
 ���� ��  � ��
 �   �]�  ��  �@Q�
IsSelected . ��  �@Q�IsSelectionActive . ./��� $	 Bd��# � �� � ���� # � �� � ��P�  � {� .  . ��� # � �� � �� P�  �@%�IsVirtualizing . . ���  � �� �� .R�$� �� .R��� $ 
Horizontal=�.+��� $��../Images/Camicon.png���� $��Center=�$��5q����� *CamID+���� .��� $ White��$	 0.5Q�� $ DarkGray��$ 1Q��� 8    �� *	CamList+.R��� $ 
Horizontal=�.+��� 
�������$��	5 5 10 10q����� *	GroupName+