   M S B A M L   `   `   `  ����  Login_SignUp     Login_SignUp.NewScanWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Title$  New Scan��5   	   $	��538��6   $	��500��6'   ���#f0f4f8 � WindowStartupLocation$ CenterScreen=�5       � 
ResizeMode$ NoResize=�5   	    � WindowStyle$
 None=�6   .��5      � .+�5
   
   �� ���#0078D4$��40��6&   $	��Top=�62   ._�5      ��  �� Orientation$ 
Horizontal=�6   $��Right=�62   $��Center=�6N   .+�5      �� $��New Scan��6   
�������$
��Bold"�6?   $��16(�6Q   $��Center=�6_   $
��Left=�6z   $
��10,0q�6�   �� 5      -   5        $��X��5      $��30��6%   $��30��60   $��	10,0,10,0q�6<   
������ 
������ 
�������$
��Bold"�5   ,   6>   5      5      �� 5   
   $��20,50,20,20q�6   .+�5      � $��0,0,0,10q�6   .+�5      �� -   5         ��Name$ ScanDetailsPlaceholder��5      $��Enter scan details��5      
�������$��Center=�5      $	��5,0q�5       J� IsHitTestVisible .  $y�Visible=�5      �� 5      -   5         ��Name$ ScanDetailsTextBox��5      $	��100��5       �� TextWrapping$
 Wrap=�5      "	 �� VerticalScrollBarVisibility$
	 Auto=�5       $��10q�5!      ���#ffffff���#0078D4$��1q�5$      5%      5&      �� 5)      $��Patient ID:��6   $��0,0,0,5q�6+   $
��Bold"�6<   �� 5+      -   5        $ PatientIdTextBox��5+      
 �� 
IsReadOnly
 . $��0,0,0,10q�5-      $��
Patient ID��5.      $��10q�5/      ���#ffffff���#0078D4$��1q�52      �� 55      $��
File Type:��6   $��0,0,0,5q�6*   $
��Bold"�6;   �� 57      $ 
Horizontal=�6   $��0,0,0,10q�62   .+�58      	� -   5         	�Name$ OutpatientRadioButton��58      $��
Outpatient��59      $��10q�5:      $��	0,10,10,0q�5;      ���#ffffff���#0078D4$��1q�5>      5?      	� 5@      -   5        $ InpatientRadioButton��5@      $��	Inpatient��5A      $��10q�5B      $��	0,10,10,0q�5C      ���#ffffff���#0078D4$��1q�5F      5G      5H      �� 5K      -   5        $��Select New Scane��5K      $��0,0,0,10q�5M      $��10q�5N      ���#0078D4
����������#005A9E$
��Bold"�5R       � Cursor$
 Hand��5S      �� 5V      -   5        $ SelectedFilePathTextBox��5V      
 . $��25��5X      $��0,0,0,10q�5Y      $��No file selected��5Z      $��10q�5[      ���#ffffff���#0078D4$��1q�5^      �� 5a      -	   5         ��Name$ SaveScanButton��5a      $��	Save Scan��5b      {�.  $��10q�5e      ���#28A745
����������#218838$
��Bold"�5i      $
 Hand��5j      5k      5l      5m      