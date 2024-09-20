import polars as pl
import pyodbc

csv_path = "MOCK_DATA_PROD3.csv"  
df = pl.read_csv(csv_path)

conn = pyodbc.connect(
    "DRIVER={SQL Server};"
    "SERVER=localhost;" 
    "DATABASE=ProductoAPI;"  
    "UID=sa;"  
    "PWD=root;"  
)
cursor = conn.cursor()

insert_query = """
INSERT INTO Productos (Nombre, Categoria, Descripcion) 
VALUES (?, ?, ?)
"""

for row in df.iter_rows(named=True):
    cursor.execute(insert_query, row['Nombre'], row['Categoria'], row['Descripcion'])

conn.commit()

cursor.close()
conn.close()