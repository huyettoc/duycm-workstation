   M S B A M L   `   `   `  ���� I  EITSWorkStation, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null9!clr-namespace:ITSWorkStation.ViewITSWorkStation.View  O,clr-namespace:ITSWorkStation.Libs.DwayneNeedITSWorkStation.Libs.DwayneNeed      ITSWorkStation.MainWindow   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     2view!clr-namespace:ITSWorkStation.View     >local,clr-namespace:ITSWorkStation.Libs.DwayneNeed     -     � Title$  
MainWindow��$	��550��$	��825�� � WindowStartupLocation$ CenterScreen=� � WindowStyle$
 None=� � WindowState$ 	Maximized=�
������  � 	Resources    MyImageSource �� 	UriSource  workstationImage  	lineStyle �� 
TargetType �� Property �� Value  BackgroundStyleStackPanel	 �� 
StartPoint
 �� EndPoint � Color � Offset  horizontalLineStyle �� BasedOn �� Path �� RelativeSource �� AncestorType  verticalLineStyle  MetroListBoxItemTemplate � ColumnDefinitions �� FallbackValue��%  &	        &	      &	 E     &	 �     &	 "    7  &	 �    7  &	 5    7  �� $ Images/usericon.png?��� $  Images/Workstationicon.png?��� " ��.���  � ��
 � �����  �@��Stretch$
 Fill=���  �@K�ZIndex$	 200���� " �.���  � �� �� $	 0.5,0&�$
 0.5,0.6&�.��� $ 	SlateGray��$ 0Q�� $ #00000f��$	 1.0Q��� " ��8   .��� 
 �@��X2$ 1����  � ��$ Bottom=���  � �� �� $ ColumnDefinitions.Count� �� $
 GridL��� " ��8   .��� 
 �@��Y2$ 1����  � ��$
 Left=���  � �� �� $ RowDefinitions.Count� �� $
 GridL��� .R��  �� $��8* ��� $��2* �.+��� $��0��$��5q�.+��� $
��Bold"����� $
 Name�$ Title���� $��1��8��  $��20��.��� -   $��mainGrid�� � RowDefinitions �� $��35 ��� $
��360* �.+��� $��0��#��[ � #��[  �� $
��571* ��� $
��246* �.+���  �� Orientation$ 
Horizontal=�.+��� -   $��liveviewtab��$��75��$y�Hidden=�$
��Left=�$��Center=�
�������$��LiveView��$
��Bold"�$��14(� �� Padding$ 10,0,0,0q��� -   $��LivevieLine��#��[ $y�Hidden=�$��18���� $ 
Horizontal=�$��0��$��35��$	��Top=�$
��Left=�.+��� -    ��Name$ LbxMenu��$
��Left=�$	��Top=�$
��Auto��$��12,0,0,0q�#��[   ListBoxItemStyle1#���  
�������$��14(� $  ListBoxWithAutoScroll_Horizontal#��[ ��Z� ��Z� ���� .R��� $ 
Horizontal=��� $��35��$��Right=�$	��220��$ 
Horizontal=�$��Center=�$��1��.+��� #��[ $��15��$��15���� 
�������$	��135��$
��Bold"�$��14(�$	��3,8q�.��Center Workstation�� #��[ $��16���� -   $��LogoutBlock��$ 
Horizontal=�$��Center=�.+��� 
�������$ 5q�	Log Out�� #��[  $��15��$��15���� -   $��userControlview��$��1������ 