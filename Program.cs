using System;
using System.Collections.Generic;
using System.IO;

class CameraStorage
{
    private const string DatabaseFile = "TextFile1.txt";

    private string lastName;
    private DateTime deliveryDate;
    private int storagePeriod;
    private int inventoryNumber;
    private string itemName;

    public string LastName
    {
        get { return lastName; }
        set { lastName = value; }
    }

    public DateTime DeliveryDate
    {
        get { return deliveryDate; }
        set { deliveryDate = value; }
    }

    public int StoragePeriod
    {
        get { return storagePeriod; }
        set { storagePeriod = value; }
    }

    public int InventoryNumber
    {
        get { return inventoryNumber; }
        set { inventoryNumber = value; }
    }

    public string ItemName
    {
        get { return itemName; }
        set { itemName = value; }
    }

    public CameraStorage()
    {

    }

    public CameraStorage(string lastName, DateTime deliveryDate, int storagePeriod, int inventoryNumber, string itemName)
    {
        this.lastName = lastName;
        this.deliveryDate = deliveryDate;
        this.storagePeriod = storagePeriod;
        this.inventoryNumber = inventoryNumber;
        this.itemName = itemName;
    }

    public void AddRecord(CameraStorage record)
    {
        try
        {
            using (StreamWriter writer = File.AppendText(DatabaseFile))
            {
                string recordString = $"{record.LastName},{record.DeliveryDate},{record.StoragePeriod},{record.InventoryNumber},{record.ItemName}";
                writer.WriteLine(recordString);
            }

            Console.WriteLine("Record added successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine("An error occurred while adding the record:");
            Console.WriteLine(ex.Message);
        }
    }

    public void EditRecord(int index, CameraStorage newRecord)
    {
        try
        {
            string[] lines = File.ReadAllLines(DatabaseFile);

            if (index >= 0 && index < lines.Length)
            {
                lines[index] = $"{newRecord.LastName},{newRecord.DeliveryDate},{newRecord.StoragePeriod},{newRecord.InventoryNumber},{newRecord.ItemName}";
                File.WriteAllLines(DatabaseFile, lines);
                Console.WriteLine("Record edited successfully.");
            }
            else
            {
                throw new IndexOutOfRangeException("Invalid record index.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("An error occurred while editing the record:");
            Console.WriteLine(ex.Message);
        }
    }

    public void DeleteRecord(int index)
    {
        try
        {
            List<string> lines = new List<string>(File.ReadAllLines(DatabaseFile));

            if (index >= 0 && index < lines.Count)
            {
                lines.RemoveAt(index);
                File.WriteAllLines(DatabaseFile, lines);
                Console.WriteLine("Record deleted successfully.");
            }
            else
            {
                throw new IndexOutOfRangeException("Invalid record index.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("An error occurred while deleting the record:");
            Console.WriteLine(ex.Message);
        }
    }

    public void DisplayRecords()
    {
        string[] lines = File.ReadAllLines("database.txt");
        foreach (string line in lines)
        {
            string[] fields = line.Split(',');
            Console.WriteLine($"Last Name: {fields[0]}");
            Console.WriteLine($"Delivery Date: {fields[1]}");
            Console.WriteLine($"Storage Period: {fields[2]}");
            Console.WriteLine($"Inventory Number: {fields[3]}");
            Console.WriteLine($"Item Name: {fields[4]}");
            Console.WriteLine();
        }
    }

    public void SearchRecords(DateTime deliveryDate)
    {
        string[] lines = File.ReadAllLines("database.txt");
        bool found = false;

        foreach (string line in lines)
        {
            string[] fields = line.Split(',');
            DateTime recordDeliveryDate;
            if (DateTime.TryParse(fields[1], out recordDeliveryDate))
            {
                if (recordDeliveryDate == deliveryDate)
                {
                    Console.WriteLine($"Last Name: {fields[0]}");
                    Console.WriteLine($"Інформація про запис зі співпадаючою датою здачі:");
                    Console.WriteLine($"Delivery Date: {fields[1]}");
                    Console.WriteLine($"Storage Period: {fields[2]}");
                    Console.WriteLine($"Inventory Number: {fields[3]}");
                    Console.WriteLine($"Item Name: {fields[4]}");
                    Console.WriteLine();
                    found = true;
                }
            }
        }

        if (!found)
        {
            Console.WriteLine("No records found with the specified delivery date.");
        }
    }

    public void SortRecords()
    {
        string[] lines = File.ReadAllLines("database.txt");
        Array.Sort(lines, new InventoryNumberComparer());

        foreach (string line in lines)
        {
            string[] fields = line.Split(',');
            Console.WriteLine($"Last Name: {fields[0]}");
            Console.WriteLine($"Delivery Date: {fields[1]}");
            Console.WriteLine($"Storage Period: {fields[2]}");
            Console.WriteLine($"Inventory Number: {fields[3]}");
            Console.WriteLine($"Item Name: {fields[4]}");
            Console.WriteLine();
        }
    }

    private class InventoryNumberComparer : IComparer<string>
    {
        public int Compare(string x, string y)
        {
            string[] fieldsX = x.Split(',');
            string[] fieldsY = y.Split(',');

            int inventoryNumberX = int.Parse(fieldsX[3]);
            int inventoryNumberY = int.Parse(fieldsY[3]);

            return inventoryNumberX.CompareTo(inventoryNumberY);
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        CameraStorage cameraStorage = new CameraStorage();
        while (true)
        {
            Console.WriteLine("Choose an option:");
            Console.WriteLine("1. Add a record");
            Console.WriteLine("2. Edit a record");
            Console.WriteLine("3. Delete a record");
            Console.WriteLine("4. Display records");
            Console.WriteLine("5. Search records by delivery date");
            Console.WriteLine("6. Sort records by inventory number");
            Console.WriteLine("Enter to exit");

            ConsoleKeyInfo key = Console.ReadKey();

            Console.WriteLine();

            switch (key.Key)
            {
                case ConsoleKey.D1:
                    Console.WriteLine("Enter Last Name:");
                    string lastName = Console.ReadLine();

                    Console.WriteLine("Enter Delivery Date (yyyy-MM-dd):");
                    DateTime deliveryDate = DateTime.Parse(Console.ReadLine());

                    Console.WriteLine("Enter Storage Period:");
                    int storagePeriod = int.Parse(Console.ReadLine());

                    Console.WriteLine("Enter Inventory Number:");
                    int inventoryNumber = int.Parse(Console.ReadLine());

                    Console.WriteLine("Enter Item Name:");
                    string itemName = Console.ReadLine();

                    CameraStorage newRecord = new CameraStorage(lastName, deliveryDate, storagePeriod, inventoryNumber, itemName);
                    cameraStorage.AddRecord(newRecord);

                    Console.WriteLine("Record added successfully.");
                    break;

                case ConsoleKey.D2:
                    Console.WriteLine("Enter the index of the record to edit:");
                    int editIndex = int.Parse(Console.ReadLine());

                    Console.WriteLine("Enter Last Name:");
                    string editedLastName = Console.ReadLine();

                    Console.WriteLine("Enter Delivery Date (yyyy-MM-dd):");
                    DateTime editedDeliveryDate = DateTime.Parse(Console.ReadLine());

                    Console.WriteLine("Enter Storage Period:");
                    int editedStoragePeriod = int.Parse(Console.ReadLine());

                    Console.WriteLine("Enter Inventory Number:");
                    int editedInventoryNumber = int.Parse(Console.ReadLine());

                    Console.WriteLine("Enter Item Name:");
                    string editedItemName = Console.ReadLine();

                    CameraStorage editedRecord = new CameraStorage(editedLastName, editedDeliveryDate, editedStoragePeriod, editedInventoryNumber, editedItemName);
                    cameraStorage.EditRecord(editIndex, editedRecord);

                    Console.WriteLine("Record edited successfully.");
                    break;
                case ConsoleKey.D3:
                    Console.WriteLine("Enter the index of the record to delete:");
                    int deleteIndex = int.Parse(Console.ReadLine());

                    cameraStorage.DeleteRecord(deleteIndex);

                    Console.WriteLine("Record deleted successfully.");
                    break;

                case ConsoleKey.D4:
                    cameraStorage.DisplayRecords();
                    break;

                case ConsoleKey.D5:
                    Console.WriteLine("Enter the delivery date to search (yyyy-MM-dd):");
                    DateTime searchDate = DateTime.Parse(Console.ReadLine());

                    cameraStorage.SearchRecords(searchDate);
                    break;

                case ConsoleKey.D6:
                    cameraStorage.SortRecords();
                    break;

                case ConsoleKey.Enter:
                    return;

                default:
                    Console.WriteLine("Invalid option. Please try again.");
                    break;
            }

            Console.WriteLine();
        }
    }
}

