   M S B A M L   `   `   `  ����I  EITSWorkStation, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null9!clr-namespace:ITSWorkStation.ViewITSWorkStation.View  5   	   O,clr-namespace:ITSWorkStation.Libs.DwayneNeedITSWorkStation.Libs.DwayneNeed  5   	       ITSWorkStation.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   2view!clr-namespace:ITSWorkStation.View     5   	   >local,clr-namespace:ITSWorkStation.Libs.DwayneNeed     5   	   -   5          � Title$  
MainWindow��5   	   $	��550��6   $	��825��6)    � WindowStartupLocation$ CenterScreen=�5	   	    � WindowStyle$
 None=�5
   	    � WindowState$ 	Maximized=�5   	   
������  � 	Resources 5         MyImageSource �� 	UriSource  workstationImage  	lineStyle �� 
TargetType �� Property �� Value  BackgroundStyleStackPanel	 �� 
StartPoint
 �� EndPoint � Color � Offset  horizontalLineStyle �� BasedOn �� Path �� RelativeSource �� AncestorType  verticalLineStyle  MetroListBoxItemTemplate � ColumnDefinitions �� FallbackValue��%
  &	        &	 -     &	 a     &	     &	 �    7  &	 �    7  &	 �    7  �� 5   
   $ Images/usericon.png?�6,   �� 5   
   $  Images/Workstationicon.png?�6/   �� 5   
   " ��6"   .�5      ��  � ��
 � ���6'   �� 5       �@��Stretch$
 Fill=�6(   �� 5       �@K�ZIndex$	 200��6,   5      �� 5   
   " �6   .�5      ��  � �� 5      �� 5      $	 0.5,0&�6*   $
 0.5,0.6&�6=   .��5      � $ 	SlateGray��6'   $ 0Q�69   � 5      $ #00000f��6'   $	 1.0Q�67   5      5      5      �� 5    
   " ��6,   8   .�5!      �� 
 �@��X2$ 1��6#   �� 5"       � ��$ Bottom=�62   �� 5#       � �� 5$      �� $ ColumnDefinitions.Count� �� $
 GridL�5'      �� 5)   
   " ��6*   8   .�5*      �� 
 �@��Y2$ 1��6#   �� 5+       � ��$
 Left=�64   �� 5,       � �� 5-      �� $ RowDefinitions.Count� �� $
 GridL�50      �� 52   
   .R�53      �  54      �� 55      $��8* �6'   �� 56      $��2* �6'   .+�58      �� $��0��6   $��5q�6-   .+�59      �� $
��Bold"�6P   ��6    �� $
 Name�$ Title��6P   5:      �� 5;      $��1��6   8��  $��20��6R   6`   5<      5=      5>      .��5?      � -   5        $��mainGrid��5?       � RowDefinitions 5@   
   �� 5B      -   5         �Name$ Topgrid��5B   (   $��35 �6   6(   �� 5C      $
��360* �6   .+�5E   
   �� $��0��6   #��[ 6   � 5F   
   #��[ 6    5G      �� 5H      $��7* �6#   �� 5I      $��3* �6#   .+�5K      ��  �� Orientation$ 
Horizontal=�6   .+�5L      �� -   5        $��liveviewtab��5L      $��75��6/   $y�Hidden=�6n   $
��Left=�6�   $��Center=�6�   
�������$��
Giám sát��6�   $
��Bold"�6�   $��14(�6�    �� Padding$ 10,0,0,0q�6�   5M      �� 5N      -   5        $��LivevieLine��5N   b   #��[ 6   $y�Hidden=�6B   $��18��6V   6w   �� 5P      $ 
Horizontal=�6   $��0��66   $��35��6F   $	��Top=�6R   $
��Left=�6j   .+�5V      �� -   5         ��Name$ LbxMenu��5W      $
��Left=�5V      $	��Top=�65   $
��Auto��6M   $��12,0,0,0q�6Z   #��[ 5X        ListBoxItemStyle1#���  5Y      
�������$��14(�5\       $  ListBoxWithAutoScroll_Horizontal#��[ 5]      ��5W   "   Z� ��68   Z� ��5_      �� 5`      .R�5a      �� $ 
Horizontal=�6)   5b      5d      5e      5f      �� 5g      $��35��6   $��Right=�6%   $	��300��6A   $ 
Horizontal=�6M   $��Center=�6f   $��1��6�   .+�5h      �� #��[ 6   $��30��6C   $��30��6N   6\   �� 5i      
�������$	��200��6/   $
��Bold"�6;   $��14(�6M   $	��3,8q�6[   .��6i   GIÁM SÁT AN NINH6{   �� 5j      #��[ 6   $��16��6B   6P   �� 5k      -   5        $��LogoutBlock��5k   Q   $ 
Horizontal=�6   $��Center=�66   .+�5l      �� 
�������$ 5q�67   Thoát6K   �� 5m      #��[  6   $��15��6D   $��15��6O   6]   5n      5p      5q      �� 5r      -   5        $��userControlview��5r   *   $��1��6   ��6A   �� 6W   5t      5u      