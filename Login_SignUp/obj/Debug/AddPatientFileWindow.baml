   M S B A M L   `   `   `  ����  Login_SignUp'    !Login_SignUp.AddPatientFileWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   -   5          � Title$  Patient Details��5   	   $	��700��6!   $	��900��6.    � WindowStartupLocation$ CenterScreen=�5   	    � 
ResizeMode$ NoResize=�5   	    � WindowStyle$ SingleBorderWindow=�5   	    � 	Resources 5	         DataGridColumnHeaderStyle �� 
TargetType=  7System.Windows.Controls.Primitives.DataGridColumnHeader �� Property �� Value��%�   &	        �� 5   
   "  62   .�5      ��  � �� �#0078D46+   �� 5       � ��
 �����6+   �� 5       � ��$
 Bold"�6+   �� 5       � ��$ 14(�6)   �� 5       � ��$ 35��6'   5      5      .��5      � $��29,0,0,0q�6    � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      �� 5      $
��281* �6   �� 5      $
��Auto �6   .+�5   
   � -   5        	 �Name$ 	 SelectedPatientDisplayGrid��5      $��0��63   $	��Top=�6@   $��
10,10,10,0q�6X    5      �� 5      $
��Auto �6    �� 5       $
��Auto �6    �� 5!      $
��Auto �6    �� 5"      $
��Auto �6    
 � ColumnDefinitions
 5$      �� 5%      $	��150 �6#   �� 5&      $��* �6#   �� 5'      $
��Auto �6#   .+�5+      �  � Orientation$ 
Horizontal=�6   $��1��61   $
��Left=�6A   $	��Top=�6\   $��0,0,10,0q�6t   .+�5,      �� $
��MRN:��6   $
��Bold"�6(   $��	10,0,10,0q�6:   �� 5-      -   5         ��Name$ MRNDisplayTextBlock��5-      �� 50      $��Name:��6   $��
60,0,10,10q�6)   $
��Bold"�6=   �� 51      -   5        $ NameDisplayTextBlock��51      �� 54      $��Patient ID:��6   $��	60,0,10,0q�6/   $
��Bold"�6B   �� 55      -   5        $ PatientIDDisplayTextBlock��55      �� 58      $��Phone Number:��6   $��	60,0,10,0q�61   $
��Bold"�6D   �� 59      -   5        $! PhoneNumberDisplayTextBlock��59      5:      �� 5=      -   5        $��Close��5=      $��16(�6%   
���  ��
�������$��0��6W   $��2��6d   $��Right=�6t   $	��Top=�6�   $��10q�6�   $	��100��6�   $��30��6�   6�   �� 5@       �� Orientation$ 
Horizontal=�6   $��4��62   $��3��6?   $��Center=�6S   $��Bottom=�6p   $��
10,10,10,0q�6�   .+�5A      �� -   5        $��New Scan��5A      
��� � �
�������$	��100��6n   $��30��6z   $��0,0,10,0q�6�   �� 5B      -	   5        $��Upload��5B      
����  �
�������$	��100��6j   $��30��6v   $��10q�6�   5C      5D      &   System.Windows.Controls.DataGrid  5G   
   -
   5          Name$ PatientFilesDataGrid��5G         AutoGenerateColumns .  $��	0,12,20,6q�5H   K   $��2��6^      ColumnHeaderStyle# [  5I         
IsReadOnly .    Columns 5K      0  *System.Windows.Controls.DataGridTextColumn  5M      ,  &System.Windows.Controls.DataGridColumn   Header$ 
Serial No.��6%      Width5  /System.Windows.Controls.DataGridLengthConverter$ * 6Z   1  +System.Windows.Controls.DataGridBoundColumn   Binding 69   �� *SerialNumber+6Z     5P      $	 MRN��6%   $ * 6Q    62   �� *
FileNumber+6Q     5Q      $
 Name��6%   $ * 6L    63   �� *Name+6L     5R      $ 
Patient ID��6%   $ * 6]    69   �� *UniquePatientID+6]     5S      $ Phone Number��6%   $ * 6[    6;   �� *PhoneNumber+6[     5T      $ 	File Name��6%   $ * 6U    68   �� *
FileName+6U     5U      $ 	File Type��6%   $ * 6U    68   �� *
FileType+6U   4  .System.Windows.Controls.DataGridTemplateColumn  5V      $ Actions��6)      CellTemplate 5W      �� 5X      .R�5Y      �� $ 
Horizontal=�6)   $��Center=�6B   .+�5Z   "   �� -   5        $
��View��5Z   )   
����  �
�������$	��100��6]   $��30��6i   $��2q�6u   6�   �� 5[   "   -   5        $��Update��5[   )   
��� � �
�������$	��100��6`   $��30��6l   $��2q�6x   6�   �� 5\   "   -   5        $��Delete��5\   )   
���  ��
�������$	��100��6^   $��30��6j   $��2q�6v   6�   5]      5^      5`      5b      5c      5d      