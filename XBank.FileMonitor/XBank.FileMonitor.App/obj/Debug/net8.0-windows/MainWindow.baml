   M S B A M L   `   `   `  ����  XBank.FileMonitor.App&     XBank.FileMonitor.App.MainWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Title$  XBank Trade Monitor��5   	   $	��550��6%   $	��800��62   .��5      � $��10q�6    � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5	      $��* �6   �� 5      $
��Auto �6   .+�5   
   ��  �� Orientation$ 
Horizontal=�6   $��0,0,0,10q�6.   .+�5      �� $��Status: ��6   $
��Bold"�6(   �� 5      -   5         ��Name$ StatusTextBlock��5      $��Ready��61   $��5,0,0,0q�6>   � 5      -   5         �Name$ LoadingProgressBar��5      $	��150��66   $��10��6B   $��20,0,0,0q�5       � IsIndeterminate$
 True��6,   $y�	Collapsed=�6C   5      &   System.Windows.Controls.DataGrid  5   
   -   5          Name$ TradeDataGrid��5      $��1��5         AutoGenerateColumns$ False��5         
IsReadOnly$
 True��5      ��5      �� *TradeDataList+	   Columns	 5      0  *System.Windows.Controls.DataGridTextColumn  5      ,  &System.Windows.Controls.DataGridColumn
   Header$

 Date��6%   1  +System.Windows.Controls.DataGridBoundColumn   Binding 63   �� *Date+ �� StringFormat$ {0:dd.MM.yyyy}��  5      $

 Open��6%    63   �� *Open+  5       $

 High��6%    63   �� *High+  5!      $	
 Low��6%    62   �� *Low+  5"      $
 Close��6%    64   �� *Close+  5#      $
 Volume��6%    65   �� *Volume+  5$      $
 Source File��6%    6:   �� *
SourceFile+5&      �� 5)   
   $��2��6   $��0,10,0,0q�6"   .+�5*      �� $��Errors:��6   $
��Bold"�6'   �� 5+      -   5         ��Name$ ErrorListBox��5+      $	��100��6,   5,      5-      5.      