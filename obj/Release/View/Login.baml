   M S B A M L   `   `   `  ����   ITSWorkStation    ITSWorkStation.View.Login   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml       � Title$  Login�� � WindowStartupLocation$ CenterScreen=� � AllowsTransparency . 
������  � WindowStyle$
 None=� � ShowInTaskbar .   � SizeToContent$ WidthAndHeight=� � 	Resources  �� 
TargetType �� Property	 �� Value
 �� 
StartPoint �� EndPoint   	backBrush � Color � Offset3  -System.Windows.Media.Effects.DropShadowEffect  shadowWindow   Color   Opacity   ShadowDepth  shadowButton��%�  '��       '�� :     'G� t     '�� �     &	  �     &	 R    &	 t    �� " ��.���  � ��$	 4q���  � ��
	 ������� " ��.���  � ��$	 4q���  � ��
	 �   ��� " G�.���  � ��$	 4q���  � ��
	 �   ��� " ��.���  � ��$	 8q���  � ��$	 36����  � ��$	 36���� $
 0.5,0.5&�$ 0.5,1&�.��� $ DarkGray��$ 0Q�� $
 Gray��$ 1Q�  $
 Gold��$	 .75Q�$ 5Q�  $ 
GhostWhite��$	 .80Q�$ 6Q�.����  �� CornerRadius$ 10��
��� ���$��4q�#��[  $��24q� �� Padding$ 5q�$	��450��$	��250��._��  � RowDefinitions �� $
��Auto ��� $
��Auto ��� $
��Auto ��� $	��25* ��� $	��85* � � ColumnDefinitions �� $	��13* ��� $	��25* ��� $	��70* �.+��� $��1��$��0��$��	4,10,4,20q�$ 10��$��2������ $ 0.5,1&�$
 0.5,0&�.��� $ 	#FFC7C2C2��$ 0Q�� $ 	#FF8D8787��$ 1Q�._��� $��12(�$��10q�$-��'Please Login To Access This Application���� $��1��$��1��$��	User Name���� -   $��TbtUserName��$��2��$��1�� �� 	MaxLength$ 25�� ^� ToolTip$ Enter Your User Name��$��4,4,0,4q�$
��Left=�$	��239���� $��1��$��2��$��Password��G� -   $��TptPassword��$��2��$��2��$ Enter Your Password��$��4,4,0,4q�$
��Left=�$	��239���� -   $��tblInfoAcount��$��2��$��3��$��Center=�Enter your account�� $��2��$��4��$��0,4,4,4q�$��Right=� �� Orientation$ 
Horizontal=�.+��� -   $��BtnLogin��$��75�� �� 	IsDefault .  J� Effect# [ $ Login��.���� $"��../Images/CheckMarkBlack.png���� -   $��	BtnCancel�� �� IsCancel . # [ $ Cancel��.���� $��../Images/XBlack.png��