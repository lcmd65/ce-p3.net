import subprocess
import threading
from tkinter import messagebox
from apscheduler.schedulers.blocking import BlockingScheduler
from main.realtime_run import time_cal
from function.function_database import *

import main.__main_db__ as mdb
import main.__main_file__ as mf

state_in_root_temp = 'end'

## Home button click to run function 10 time of runnning 2 main file, and draw plot to compare runtime
def clicked_home():
    time_cal().show()

## Function call main function in 2 file processing file and processing database
def processing_cal_unlist(txt,type_check):
    processing_type(type_check)
    temp_click(txt)
    print("success")

## Function for button auto run in tab LASER P3A __denied
def clicked_function_schedule(txt, type_check):
    sched = BlockingScheduler()
    sched.configure(max_instances = 10, name='Alternate name')
    @sched.scheduled_job('interval', seconds=20, max_instances= 6)
    def ui_schedule():
        processing_cal_unlist(txt, type_check)
    sched.start()
    
def state_check_thread(txt,type_check):
    t1 = threading.Thread(target=state_check(txt, type_check), )
    t1.start()
    t1.join()
    
def state_check(txt,type_check):
    try:
        processing_cal_unlist(txt,type_check)
    except:
        messagebox.showinfo(title= "CE Message", message ="Error in connect")

##_________button click in user interface main____thread & subprocess design 
def temp_click(txt): 
    txt.insert('1.0', "process successful ")

## Change the state in clicking run in root 2


##_________Main funtion auto tracking in new thread
def click_function_schedule_thread(txt, type_check):
    try:
        t1 = threading.Thread(target=clicked_function_schedule(txt, type_check), )
        t1.start()
        t1.join()
    except:
        print ("error")

##_________Main function auo tracking in new subprocess(thread())
def sub_click_thread(txt, type_check):
    temp = subprocess.Popen(click_function_schedule_thread(txt, type_check), shell= True)
    return temp.communicate()

##_________ket hop mo mot luong song song va chạy subprocess luong do
def sub_click_thread_temp(txt):
    temp = subprocess.Popen(temp_click(txt), shell= True)
    return temp.communicate()

##_________Running 2+ funtion in event
def sequence(*functions):
    def func(*args, **kwargs):
        return_value = None
        for function in functions:
            return_value = function(*args, **kwargs)
        return return_value
    return func
