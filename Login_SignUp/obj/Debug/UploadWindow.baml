   M S B A M L   `   `   `  ����G  CLogin_SignUp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null+clr-namespace:Login_SignUpLogin_SignUp  5   	       Login_SignUp.UploadWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   ,localclr-namespace:Login_SignUp     5   	     � Title$  UploadWindow��5   	   $	��300��6   $	��400��6+    � WindowStartupLocation$ CenterScreen=�67   .��5	      � .+�5
   
   �� $��20q�6   .+�5      �� -   5        $��Select File��5      $��10q�5      ���#0078D4
����������#005A9E$
��Bold"�5       � Cursor$
 Hand��5      �� 5      -   5         ��Name$ SelectedFilePathTextBox��5       �� 
IsReadOnly . $��39��5      $
��0,10q�5      $��No file selected��5      $��10q�5      ���#ffffff���#0078D4$��1q�5      �� 5!      $��
File Type:��6   $��0,10,0,5q�6*   �� 5"       �� Orientation$ 
Horizontal=�6   $��0,0,0,10q�62   .+�5#      	� -   5         	�Name$ InPatientRadioButton��5#      $��
In Patient��5$       	� 	GroupName$ FileType��5%      $��0,0,10,0q�5&      	� 5'      -   5        $ OutPatientRadioButton��5'      $��Out Patient��5(      $ FileType��5)      5*      �� 5-      -   5        $
��Save��5-      $��10q�5/      ���#28A745
����������#218838$
��Bold"�53      $
 Hand��54      $��0,10,0,0q�55      56      57      58      