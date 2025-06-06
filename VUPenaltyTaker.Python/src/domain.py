from abc import ABC, abstractmethod
from dataclasses import dataclass
from enum import Enum
from pathlib import Path
from typing import List, Union

from matplotlib.figure import Figure


class Direction(Enum):
    LEFT = "Left"
    RIGHT = "Right"
    NONE = "None"

    @classmethod
    def from_string(cls, direction_str: str):
        try:
            return cls[direction_str.upper()] if direction_str else cls.NONE
        except KeyError:
            return cls.NONE


@dataclass
class TrialData:
    participant: int
    trial: int
    goalkeeper_displacement: float
    shot_location_x: float
    goalkeeper_movement_initiation: float
    shot_direction: Direction
    jump_direction: Direction
    advertisement_direction: Direction


class Condition(Enum):
    ADVERTISEMENTANDGOAL = "AdvertisementAndGoal"
    ADVERTISEMENTONLY = "AdvertisementOnly"
    ONLYADD = "OnlyAdd"

    @classmethod
    def from_string(cls, condition_str: str):
        try:
            return cls[condition_str.upper()]
        except KeyError:
            return None


@dataclass
class TrialDataExperiment3And4:
    participant: int
    trial: int
    advertisement_direction: Direction
    goalkeeper_position: float
    condition: Condition


@dataclass
class Table:
    title: str
    header: List[str]
    rows: List[List[Union[str, float]]]

    def rename_element(self, original: str, new: str):
        if original in self.header:
            self.header[self.header.index(original)] = new
        for row in self.rows:
            if original in row:
                row[row.index(original)] = new

    def reorder_element(self, cell_content: str, new_row_id: int):
        # Separate rows containing `name` and those that do not
        matching_rows = [row for row in self.rows if cell_content in row]
        non_matching_rows = [row for row in self.rows if cell_content not in row]
        self.rows = non_matching_rows[:new_row_id] + matching_rows + non_matching_rows[new_row_id:]

    def __post_init__(self):
        for row in self.rows:
            if len(row) != len(self.header):
                raise Exception(
                    f"Number of columns in each row must match the number of header elements: error in row: {row}")


class ColumnFormat(Enum):
    SINGLE = 1
    DOUBLE = 2


class Persistence(ABC):

    @abstractmethod
    def save_table(self, table: Table, filename: Path, column_format: ColumnFormat = ColumnFormat.DOUBLE) -> None:
        ...

    @abstractmethod
    def save_figure(self, figure: Figure, filename: Path) -> None:
        ...

    @abstractmethod
    def figure_width(self, column_format: ColumnFormat) -> float:
        ...

    @abstractmethod
    def save_model(self, model: str, file_name: Path) -> None:
        ...

    @abstractmethod
    def save_outliers(self, outliers: List[TrialDataExperiment3And4], file_name: Path) -> None:
        ...
