   M S B A M L   `   `   `  ���� D  @AddInView, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null8*clr-namespace:AddInView;assembly=AddInView	AddInView  I ELiveViewPlugin, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null9!clr-namespace:LiveViewPlugin.ViewLiveViewPlugin.View 9!clr-namespace:LiveViewPlugin.ViewLiveViewPlugin.View -   'LiveViewPlugin.View.LiveViewUsercontrol   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     A
addInViews*clr-namespace:AddInView;assembly=AddInView     c _DevExpress.Xpf.NavBar.v12.2, Version=12.2.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a>dxn4http://schemas.devexpress.com/winfx/2008/xaml/navbar  ,view!clr-namespace:LiveViewPlugin.View  -view1!clr-namespace:LiveViewPlugin.View    � 	Resources     CommonItemStyle �� Property �� Value��%@   &	        �� .���  � ��$ 14(���  � ��
 �����.��� -   $��MainGrid��
������� � ColumnDefinitions �� -    �Name$ AbColumnDefinition��$��6* ��� $	��40* �.+�� -   $
��left��
������ .+�)  #DevExpress.Xpf.NavBar.NavBarControl  -   $��navbar��$
��Left=�$	��100��   �� Source�� $M Gpack://application:,,,/LiveViewPlugin;component/Styles/NavBarTheme.xaml?�%       Groups '  !DevExpress.Xpf.NavBar.NavBarGroup  -   $ navBarGroup1��   Headera ]DevExpress.Xpf.Core.v12.2, Version=12.2.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a)  #DevExpress.Xpf.Core.ObjectConverter$ Camera    DisplaySource$ Content=�	   GroupScrollMode$
	 NoneX�
   Content
 R� -   $	��100��$��0q�
������   TreeViewItemStyle#���    DepartmentTemplate#���  $��Hidden=����� *CamGroupCollection+  �� $V Ppack://application:,,,/LiveViewPlugin;component/Styles/CommonTreeViewStyles.xaml?�%      -   $ navBarGroup2��$	 XXX $ Content=�
 �  � ShowGridLines .  � RowDefinitions �� $��25 ��� $	��150 ��� .+��� $��Center=�$��Center=�$��40��$��20��$��14(�$
��Bold"�
�������$��0q�.��PTZ�� $��1��.+��� $��32��$��30��$
��5,15q�$
��Left=�$	��Top=����� .R��� .+��� $��../Images/Controler/1.png���� $��32��$��30��$��35,15q�$
��Left=�$	��Top=����� .R��� .+��� $��../Images/Controler/3.PNG���� $��32��$��30��$��67,15q�$
��Left=�$	��Top=����� .R��� .+��� $��../Images/Controler/2.PNG���� $��29��$��38��$
��5,45q�$
��Left=�$	��Top=����� .R��� .+��� $��../Images/Controler/8.PNG���� $��32��$��30��$
��5,83q�$
��Left=�$	��Top=����� .R��� .+��� $��../Images/Controler/7.PNG���� $��37��$��26��$��34,87q�$
��Left=�$	��Top=����� .R��� .+��� $��../Images/Controler/6.PNG���� $��33��$��29��$��71,84q�$
��Left=�$	��Top=����� .R��� .+��� $��../Images/Controler/5.PNG���� $��26��$��38��$��72,44q�$
��Left=�$	��Top=����� .R��� .+��� $��../Images/Controler/4.PNG���� $��47��$��46��$��30,39q�$
��Left=�$	��Top=����� .R��� .+��� $��../Images/Controler/9.PNG��   View '  !DevExpress.Xpf.NavBar.SideBarView  *  $DevExpress.Xpf.NavBar.NavBarViewBase   
Background
 � ���-  'DevExpress.Xpf.NavBar.ScrollingSettings   
ScrollMode$
 None=�   GroupVisualStyle# [  � $��1�� .  �� -    ��Name$
 col1��$��* ��� $��* � �� -	    ��Name$
 row1��$��* ��� $��* �.+�0  *LiveViewPlugin.View.VideoScreenUserControl  -
     Name$ ucVideoScreen1��0  *LiveViewPlugin.View.VideoScreenUserControl   	MouseDown$  UcVideoScreen1_OnMouseDown��$��0��$��0��  -   $ ucVideoScreen2��$  UcVideoScreen2_OnMouseDown��$��0��$��1��  -   $ ucVideoScreen3��$  UcVideoScreen3_OnMouseDown��$��0��$��1��  -   $ ucVideoScreen4��$  UcVideoScreen4_OnMouseDown��$��1��$��1��