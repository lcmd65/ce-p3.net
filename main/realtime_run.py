from apscheduler.schedulers.blocking import BlockingScheduler
import main.__main_db__ as mdb
import main.__main_file__ as mf
import time
import pandas as pd
import matplotlib.pyplot as plt
from tkinter import messagebox

list_db = []
list_file = []
list_total = []
count_val = 0

## for running without timecal
def processing_cal_unlist():
    mdb.main()
    mf.main()

## prcessing in caculate time run
def processing_cal():
    print("____________Start____________")
    start = time.time()
    mdb.main()
    elapsed = time.time() - start
    list_db.append(elapsed)
    start = time.time()
    mf.main()
    elapsed = time.time() - start
    list_file.append(elapsed)
    start = time.time()
    mdb.main()
    mf.main()
    elapsed = time.time() - start
    list_total.append(elapsed)

## funtion for drawwing runtime plot
def time_cal():
    list_db.clear()
    list_file.clear()
    list_total.clear()
    for i in range(10):
        processing_cal()
    if len(list_db) == 10:
        data_temp = [list_db, list_file, list_total]
        df_temp = pd.DataFrame(data_temp)
        
        plt.subplot(2, 2, 1)
        plt.title('Runtime of database processing', fontsize=17)
        plt.xlabel('X', fontsize=16)
        plt.ylabel('time', fontsize=16)
        plt.grid()
        plt.plot(df_temp.iloc[0, :], color='skyblue', linewidth=5)

        plt.subplot(2, 2, 2)
        plt.title('Runtime of file processing', fontsize=17)
        plt.xlabel('X', fontsize=16)
        plt.ylabel('time', fontsize=16)
        plt.grid()
        plt.plot(df_temp.iloc[1, :], color='skyblue', linewidth=5)
        
        plt.subplot(2, 1, 2)
        plt.title('Runtime of combination process', fontsize=17)
        plt.xlabel('X', fontsize=16)
        plt.ylabel('time', fontsize=16)
        plt.grid()
        plt.plot(df_temp.iloc[2, :], color='skyblue', linewidth=5)
    return plt

## python3 main/realtime_run.py
def main():
    sched = BlockingScheduler()
    @sched.scheduled_job('interval', seconds=30)
    def timed_job():
        processing_cal_unlist()
    sched.configure(name='Alternate name')
    sched.start()


## @sched.scheduled_job('cron', day_of_week='mon-fri', hour=10)
##def scheduled_job():
    ##main()

if __name__ == "__main__":
     main()

## python3 main/realtime_run.py