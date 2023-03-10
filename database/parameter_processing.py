
import pymssql
from database.database_connect_string import *
from database.raw_data_string import *

## Push actual value to DB after get data from XML SChema
def push_db_actual(df,type):
    cnxn = pymssql.connect(server= HOST, port= PORT\
                        , database= DB_PUSH\
                        , user= USER\
                        , password= PASSWORD)
    cursor = cnxn.cursor()
    cursor.execute("SELECT * FROM " + TABLE_ACTUAL_A)
    if type == "A":
        for index in range (size_df):
            cursor.execute("""INSERT INTO """ + TABLE_ACTUAL_A + """ (ITEM, CATALOG_NAME, PARA_NAME, POR_VALUE, PRIORITY_VALUE, TOOL_VALUE) VALUES (%s, %s, %s, %s, %s, %s)""",\
                (str(df.iloc[index, 0]),\
                str(df.iloc[index, 1]), \
                str(df.iloc[index, 2]), \
                str(df.iloc[index, 3]), \
                str(df.iloc[index, 4]), \
                str(df.iloc[index, 7])))
            cnxn.commit()
    elif type == "B":
        for index in range (size_df):
            cursor.execute("""INSERT INTO """ + TABLE_ACTUAL_B + """ (ITEM, CATALOG_NAME, PARA_NAME, POR_VALUE, PRIORITY_VALUE, TOOL_VALUE) VALUES (%s, %s, %s, %s, %s, %s)""",\
                (str(df.iloc[index, 0]),\
                str(df.iloc[index, 1]), \
                str(df.iloc[index, 2]), \
                str(df.iloc[index, 3]), \
                str(df.iloc[index, 4]), \
                str(df.iloc[index, 7])))
            cnxn.commit()
    elif type == "C":
        for index in range (size_df):
            cursor.execute("""INSERT INTO """ + TABLE_ACTUAL_C + """ (ITEM, CATALOG_NAME, PARA_NAME, POR_VALUE, PRIORITY_VALUE, TOOL_VALUE) VALUES (%s, %s, %s, %s, %s, %s)""",\
                (str(df.iloc[index, 0]),\
                str(df.iloc[index, 1]), \
                str(df.iloc[index, 2]), \
                str(df.iloc[index, 3]), \
                str(df.iloc[index, 4]), \
                str(df.iloc[index, 7])))
            cnxn.commit()
    cnxn.close()