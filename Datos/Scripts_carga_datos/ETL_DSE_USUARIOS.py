import polars as pl
import pyodbc

csv_path = "MOCK_DATA3.csv"  
df = pl.read_csv(csv_path)

conn = pyodbc.connect(
    "DRIVER={SQL Server};"
    "SERVER=localhost;" 
    "DATABASE=UsuarioAPI;"  
    "UID=sa;"  
    "PWD=root;"  
)
cursor = conn.cursor()

insert_query = """
INSERT INTO Usuario (Name, Email, Age, Password) 
VALUES (?, ?, ?, ?)
"""

for row in df.iter_rows(named=True):
    cursor.execute(insert_query, row['Name'], row['Email'], row['Age'], row['Password'])

conn.commit()

cursor.close()
conn.close()