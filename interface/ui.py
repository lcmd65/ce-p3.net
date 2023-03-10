from function_ui import *
from functools import partial
from tkinter.ttk import *
import tksheet 
from tkinter import *
from function.function_database import *
from PIL import Image, ImageTk
from function.function_compare import *

signal_loop = 1

def loop():
    print("loop")

def click_run_root_temp():
    global state_in_root_temp
    global signal_loop
    state_in_root_temp = 'run'
    signal_loop = 1
    messagebox.showinfo(title= "CE Message", message="Start auto")

def click_exit_root_temp():
    global state_in_root_temp
    global signal_loop
    state_in_root_temp = 'end'
    signal_loop = 0

def start_loop(txt, type_check):
    global state_in_root_temp 
    global signal_loop
    if state_in_root_temp == "run":
        state_check_thread(txt, type_check)
    if signal_loop == 1:
        root.after(10000,sequence(partial(start_loop,txt,type_check), loop))

##_________running variable checking in new app root
def clicked_function_app_schedule(txt, type_check):
    global state_in_root_temp
    global signal_loop
    signal_loop = 1
    root_temp= Tk()
    root_temp.geometry("400x80+300+300")
    app_temp = Checking_running(root_temp)
    start_loop(txt,type_check)
    root_temp.mainloop()

##start a new thread to run schefule in app
def clicked_function_app_schedule_thread(txt, type_check):
    temp_1 = threading.Thread(target= clicked_function_app_schedule(txt, type_check), )
    temp_1.start()
    temp_1.join()

def view1(txt, sheet, type_check):
            df = processing_unpush(type_check)
            sheet.set_sheet_data(data = df.values.tolist(),\
                reset_col_positions = True,\
                reset_row_positions = True,\
                redraw = True,\
                verify = False,\
                reset_highlights = False)
            trigger(sheet)
            temp_click(txt)

def trigger(sheet_temp):
    for i in range(sheet_temp.get_total_rows()):
        if compare_string(sheet_temp.get_cell_data(i, 3, return_copy = True), sheet_temp.get_cell_data(i, 7,  return_copy = True), sheet_temp.get_cell_data(i, 4,  return_copy = True)) == 0:
            sheet_temp.highlight_cells(row = i, column = 7, bg = "Red", fg = None, redraw = False, overwrite = True)
    # get_cell_data(r, c, return_copy = True)
    # highlight_cells(row = 0, column = 0, cells = [], canvas = "table", bg = None, fg = None, redraw = False, overwrite = True)

## UI of Laser python CE P3
class Example(Frame):
    def __init__(self, parent):
        Frame.__init__(self, parent)
        self.parent = parent
        self.initUI()
    
    def initUI(self):
        self.parent.title("CE LASER P3")
        self.pack(fill=BOTH, expand=True)
        
        label = Label(self, i= bg1)
        label.pack()
        
        # Tab
        tab_control = Notebook(label)
        
        tab0 = Frame(tab_control)
        tab0.pack(side= LEFT, padx=0, pady=5)
        tab1 = Frame(tab_control)
        tab1.pack(side= LEFT, padx=0, pady=5)
        tab2 = Frame(tab_control)
        tab2.pack(side= LEFT, padx=0, pady=5)
        tab3 = Frame(tab_control)
        tab3.pack(side = LEFT, padx=0, pady=5)
        
        tab_control.add(tab0, text='HOME')
        tab_control.add(tab1, text='Laser P3A')
        tab_control.add(tab2, text='Laser P3B')
        tab_control.add(tab3, text='Laser P3C')
        
        tab_control.pack(expand= True, fill=BOTH, padx=40, pady= 0)
          
        canvas1 = Canvas(tab0)
        canvas1.pack(fill = BOTH, expand= 1)
        canvas1.create_image( 0, 0,  anchor = "nw")
        
        # HOME
        
        frame_tab0_1 = Frame(canvas1)
        frame_tab0_1.pack(fill= X, padx=5 ,pady=5)
        
        Button_tab0_0 = Button(frame_tab0_1, text="Analyze", width=10, command = sequence(clicked_home))
        Button_tab0_0.pack(side=LEFT, padx=5, pady=5)
        
        # P3A
        frame1a = Frame(tab1)
        frame1a.pack(fill=X)
        frame2a = Frame(tab1)
        frame2a.pack(fill=X)
        frame3a = Frame(tab1)
        frame3a.pack(side= LEFT, fill=Y, pady=5, padx =5)
        
        sheet1 =tksheet.Sheet(frame3a, data = [[]], height = 800, width = 1500)
        sheet1.pack(fill=BOTH, pady=10, padx=5, expand=True)
        sheet1.grid(row =20, column = 20,sticky="nswe")
        sheet1.enable_bindings()
        
        txt = Text(frame2a, bg ="#fcfcfc", height= 2)
        txt.pack(fill=BOTH, pady=0, padx=5, expand=True)
    
        Button_tab1_1 = Button(frame1a, text="Laser P3 Tracking", width=10, command = sequence(partial(processing_cal_unlist,txt, "A")))
        Button_tab1_1.pack(side=LEFT, padx=5, pady=5)
        Button_tab1_2 = Button(frame1a, text="End Auto", width=10, command = sequence(click_exit_root_temp))
        Button_tab1_2.pack(side=RIGHT, padx=5, pady=5)
        Button_tab1_3 = Button(frame1a, text="Auto", width =10,  command = sequence(partial(clicked_function_app_schedule, txt, "A")))
        Button_tab1_3.pack(side=RIGHT, padx=5, pady=5)
        Button_tab1_4 = Button(frame1a, text="View", width =10, command= partial(view1,txt,sheet1, "A"))
        Button_tab1_4.pack(side=LEFT, padx=5, pady=5)
        
        # P3B
        frame1b = Frame(tab2)
        frame1b.pack(fill=X)
        frame2b = Frame(tab2)
        frame2b.pack(fill=X)
        frame3b = Frame(tab2)
        frame3b.pack(side= LEFT, fill=Y, pady=5, padx =5)
        
        sheet2 =tksheet.Sheet(frame3b, data = [[]], height = 800, width = 1500)
        sheet2.pack(fill=BOTH, pady=10, padx=5, expand=True)
        sheet2.grid(row =20, column = 20,sticky="nswe")
        sheet2.enable_bindings()
        
        text_b = Text(frame2b,  height= 2)
        text_b.pack(fill = X)
        Button_tab2_1 = Button(frame1b, text="Laser P3 Tracking", width =10, command = partial(processing_cal_unlist,text_b, "B"))
        Button_tab2_1.pack(side =LEFT, padx=5, pady=5)
        Button_tab2_2 = Button(frame1b, text="End Auto", width =10, command= click_exit_root_temp)
        Button_tab2_2.pack(side =RIGHT, padx=5, pady=5)
        Button_tab2_3 = Button(frame1b, text="Auto", width =10,  command = sequence(partial(clicked_function_app_schedule, text_b, "B")))
        Button_tab2_3.pack(side=RIGHT, padx=5, pady=5)
        Button_tab2_4 = Button(frame1b, text="View", width =10, command= partial(view1,text_b, sheet2, "B"))
        Button_tab2_4.pack(side =LEFT, padx=5, pady=5)
        # P3C
        
        frame1c = Frame(tab3)
        frame1c.pack(fill=X)
        frame2c = Frame(tab3)
        frame2c.pack(fill=X)
        frame3c = Frame(tab3)
        frame3c.pack(side= LEFT, fill=Y, pady=5, padx =5)
        
        sheet3 =tksheet.Sheet(frame3c, data = [[]], height = 800, width = 1500)
        sheet3.pack(fill=BOTH, pady=10, padx=5, expand=True)
        sheet3.grid(row =20, column = 20,sticky="nswe")
        sheet3.enable_bindings()
        
        text_c = Text(frame2c,  height= 2)
        text_c.pack(fill =X)
        
        Button_tab3_1 = Button(frame1c, text="Laser P3 Tracking", width =10, command = partial(processing_cal_unlist,text_c, "C"))
        Button_tab3_1.pack(side =LEFT, padx=5, pady=5)
        Button_tab3_2 = Button(frame1c, text="End Auto", width =10, command = click_exit_root_temp)
        Button_tab3_2.pack(side =RIGHT, padx=5, pady=5)
        Button_tab3_3 = Button(frame1c, text="Auto", width =10,  command = sequence(partial(clicked_function_app_schedule, text_c, "C")))
        Button_tab3_3.pack(side=RIGHT, padx=5, pady=5)
        Button_tab3_4 = Button(frame1c, text="View", width =10, command= partial(view1,text_c, sheet3, "C"))
        Button_tab3_4.pack(side =LEFT, padx=5, pady=5)
        

class Checking_running(Frame):
    def __init__(self, parent): 
        Frame.__init__(self, parent)
        self.parent = parent
        self.initUI()
    
    def initUI(self):
        self.parent.title("CE LASER P3 AUTO RUNNING")
        self.pack(fill=BOTH, expand=True)
        
        frame1 = Frame(self)
        frame1.pack(fill = X)
        frame2 = Frame(self)
        frame2.pack(fill= BOTH)
        
        Button1 = Button(frame1, text ="Run",width =15, command = click_run_root_temp )
        Button1.pack(side = BOTTOM, padx =5, pady=5)
        
        Button2 = Button(frame2, text="End", width =5, command = sequence(click_exit_root_temp, exit))
        Button2.pack(side = RIGHT, padx=5, pady =5)

if __name__ == "__main__" :
    root = Tk()
    root.geometry('1200x600+200+200') 
    # Turn off the window shadow
    # Set the root window background color to a transparent color
    bg1 = ImageTk.PhotoImage(Image.open('raw_data/images/FS_image.png'))
    bg2 = ImageTk.PhotoImage(Image.open('raw_data/images/FS_image5.png'))
    app= Example(root)
    root.mainloop()

# python3 interface/ui.py